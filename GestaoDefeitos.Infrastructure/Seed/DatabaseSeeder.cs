using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Entities.Events;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Text;

namespace GestaoDefeitos.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Contributor>>();

        // Ensure database exists/migrated (caller already migrates, just in case)
        await db.Database.MigrateAsync(cancellationToken);

        // 1) Ensure main user exists
        const string mainEmail = "tccbes@pucpr.br";
        const string mainPassword = "123";

        var mainUser = await userManager.FindByEmailAsync(mainEmail);
        if (mainUser is null)
        {
            mainUser = new Contributor
            {
                Id = Guid.NewGuid(),
                Email = mainEmail,
                UserName = mainEmail,
                EmailConfirmed = true,
                Firstname = "Bruno",
                Lastname = "Esteves",
                Department = "TI",
                ContributorProfession = ContributorProfession.Developer
            };

            var createRes = await userManager.CreateAsync(mainUser, mainPassword);
            if (!createRes.Succeeded)
            {
                var errors = string.Join(", ", createRes.Errors.Select(e => $"{e.Code}:{e.Description}"));
                throw new InvalidOperationException($"Failed to create seed user {mainEmail}: {errors}");
            }
        }

        // 2) Ensure some additional contributors exist (used to assign defects / comments)
        async Task<Contributor> EnsureContributorAsync(string email, string first, string last, ContributorProfession prof)
        {
            var u = await userManager.FindByEmailAsync(email);
            if (u != null) return u;
            u = new Contributor
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserName = email,
                EmailConfirmed = true,
                Firstname = first,
                Lastname = last,
                Department = "TI",
                ContributorProfession = prof
            };
            var r = await userManager.CreateAsync(u, "123");
            if (!r.Succeeded)
            {
                var errors = string.Join(", ", r.Errors.Select(e => $"{e.Code}:{e.Description}"));
                throw new InvalidOperationException($"Failed to create seed user {email}: {errors}");
            }
            return u;
        }

        var tester = await EnsureContributorAsync("ana.tester@pucpr.br", "Ana", "Silva", ContributorProfession.Tester);
        var dev2 = await EnsureContributorAsync("carlos.dev@pucpr.br", "Carlos", "Souza", ContributorProfession.Developer);

        // 3) Ensure Projects
        var erpName = "Sistema ERP Empresarial";
        var erpDescription = RemoveDiacritics("Plataforma integrada de gestão empresarial (vendas, estoque, financeiro)");
        // Sanitize to ASCII to avoid environments that render Unicode incorrectly
        var pontoName = RemoveDiacritics("Sistema de Ponto Eletrônico");
        var pontoDescription = RemoveDiacritics("Sistema de marcação de ponto eletrônico e apuração de jornada");

        var erp = await db.Projects.FirstOrDefaultAsync(p => p.Name == erpName, cancellationToken);
        if (erp is null)
        {
            erp = new Project
            {
                Id = Guid.NewGuid(),
                Name = erpName,
                Description = erpDescription,
                CreatedAt = DateTime.UtcNow.AddDays(-120)
            };
            db.Projects.Add(erp);
        }

        var ponto = await db.Projects.FirstOrDefaultAsync(p => p.Name == pontoName, cancellationToken);
        if (ponto is null)
        {
            ponto = new Project
            {
                Id = Guid.NewGuid(),
                Name = pontoName,
                Description = pontoDescription,
                CreatedAt = DateTime.UtcNow.AddDays(-60)
            };
            db.Projects.Add(ponto);
        }

        await db.SaveChangesAsync(cancellationToken);

        // 4) Ensure Project-Contributors with roles
        async Task EnsureProjectContributorAsync(Project project, Contributor contributor, ProjectRole role)
        {
            var exists = await db.ProjectContributors.AnyAsync(pc => pc.ProjectId == project.Id && pc.ContributorId == contributor.Id, cancellationToken);
            if (!exists)
            {
                db.ProjectContributors.Add(new ProjectContributor
                {
                    ProjectId = project.Id,
                    ContributorId = contributor.Id,
                    Role = role,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                });
            }
        }

        await EnsureProjectContributorAsync(erp, mainUser, ProjectRole.Administrator);
        await EnsureProjectContributorAsync(erp, dev2, ProjectRole.Contributor);
        await EnsureProjectContributorAsync(erp, tester, ProjectRole.Contributor);

        await EnsureProjectContributorAsync(ponto, mainUser, ProjectRole.Contributor);
        await EnsureProjectContributorAsync(ponto, dev2, ProjectRole.Administrator);
        await EnsureProjectContributorAsync(ponto, tester, ProjectRole.Contributor);

        await db.SaveChangesAsync(cancellationToken);

        // 5) Seed Defects if not already populated
        var rng = new Random(1234);

        async Task<List<Defect>> EnsureDefectsForProjectAsync(Project project, int targetCount)
        {
            var existing = await db.Defects.Where(d => d.ProjectId == project.Id).ToListAsync(cancellationToken);
            if (existing.Count >= targetCount) return existing;

            var statuses = new[] { DefectStatus.New, DefectStatus.InProgress, DefectStatus.WaitingForUser, DefectStatus.Resolved, DefectStatus.Invalid, DefectStatus.Reopened };
            var categories = new[] { DefectCategory.Functional, DefectCategory.Interface, DefectCategory.Performance, DefectCategory.Improvement };
            var severities = new[] { DefectSeverity.VeryHigh, DefectSeverity.High, DefectSeverity.Medium, DefectSeverity.Low, DefectSeverity.VeryLow };
            var priorities = new[] { DefectPriority.P1, DefectPriority.P2, DefectPriority.P3, DefectPriority.P4, DefectPriority.P5 };
            var environments = new[] { DefectEnvironment.Development, DefectEnvironment.Staging, DefectEnvironment.Production };

            string[] erpSummariesRaw = new[]
            {
                "Erro ao calcular impostos na nota fiscal",
                "Lentidão no relatório de vendas mensais",
                "Falha ao exportar planilha de estoque",
                "Tela de cadastro de clientes não valida CPF",
                "Dashboard financeiro não atualiza em tempo real",
                "Erro 500 ao fechar pedido com desconto",
                "Permissão incorreta no módulo de compras",
                "Quebra de layout no cadastro de produtos",
                "Timeout ao sincronizar com gateway de pagamentos",
                "Duplicidade de lançamentos no contas a pagar",
                "Filtros de busca por período não funcionam",
                "Cálculo de comissão divergente",
                "Exportação de XML para SEFAZ falha",
                "Upload de anexo em ordem de serviço falha",
                "Integração com ERP legado retorna 401",
                "Gráfico de estoque mínimo exibe valores errados",
                "Erro ao gerar boleto bancário",
                "Auditoria não registra alterações de preço",
                "API de produtos devolve 404 para SKU válido",
                "Atualização de versão trava em 75%"
            };
            var erpSummaries = erpSummariesRaw.Select(RemoveDiacritics).ToArray();

            string[] pontoSummaries = new[]
            {
                RemoveDiacritics("Justificativa de falta não salva"),
                RemoveDiacritics("Exportação de espelho de ponto em PDF quebra"),
                RemoveDiacritics("Atraso no cálculo de horas extras")
            };

            var summaries = project.Name.Contains("ERP") ? erpSummaries : pontoSummaries;

            var createdDefects = new List<Defect>();
            var countToCreate = Math.Max(0, targetCount - existing.Count);

            for (int i = 0; i < countToCreate && i < summaries.Length; i++)
            {
                var status = statuses[i % statuses.Length];
                var category = categories[rng.Next(categories.Length)];
                var severity = severities[rng.Next(severities.Length)];
                var priority = priorities[rng.Next(priorities.Length)];
                var env = environments[rng.Next(environments.Length)];

                var createdAt = project.CreatedAt.AddDays(rng.Next(5, 110)).AddHours(rng.Next(0, 23));
                var expiresIn = createdAt.AddDays(rng.Next(7, 45));

                var assignee = (i % 2 == 0) ? mainUser : (i % 3 == 0 ? tester : dev2);

                var summaryText = summaries[i];
                var descriptionText = RemoveDiacritics($"{summaryText} - Passos para reproduzir: ...");

                var defect = new Defect
                {
                    Id = Guid.NewGuid(),
                    ProjectId = project.Id,
                    AssignedToContributorId = assignee.Id,
                    Summary = summaryText,
                    Description = descriptionText,
                    DefectCategory = category,
                    DefectSeverity = severity,
                    DefectEnvironment = env,
                    DefectPriority = priority,
                    Version = project.Name.Contains("ERP") ? $"v1.{i % 4}.{rng.Next(0, 9)}" : $"v2.{i % 2}.{rng.Next(0, 9)}",
                    ExpectedBehaviour = RemoveDiacritics("Processo deve executar conforme especificações"),
                    ActualBehaviour = "Comportamento divergente observado",
                    ErrorLog = i % 4 == 0 ? RemoveDiacritics("System.TimeoutException: operação excedeu o tempo limite") : string.Empty,
                    ExpiresIn = expiresIn,
                    Status = status,
                    CreatedAt = createdAt,
                };

                // Tags
                var tags = new[] { "backend", "frontend", "api", "database", "ui", "performance", "bug", "improvement" };
                var tagCount = 1 + rng.Next(0, 3);
                for (int t = 0; t < tagCount; t++)
                {
                    defect.Tags.Add(new Tag
                    {
                        Id = Guid.NewGuid(),
                        Description = tags[(i + t) % tags.Length],
                        CreatedAt = createdAt.AddMinutes(10 + t)
                    });
                }

                // Comments
                defect.Comments.Add(new DefectComment
                {
                    Id = Guid.NewGuid(),
                    ContributorId = tester.Id,
                    Content = RemoveDiacritics("Comentário inicial: validado cenário e reproduzido"),
                    CreatedAt = createdAt.AddHours(2)
                });
                if (i % 3 == 0)
                {
                    defect.Comments.Add(new DefectComment
                    {
                        Id = Guid.NewGuid(),
                        ContributorId = dev2.Id,
                        Content = RemoveDiacritics("Em análise pela equipe de desenvolvimento"),
                        CreatedAt = createdAt.AddHours(5)
                    });
                }

                // Attachment (some defects)
                if (i % 5 == 0)
                {
                    defect.Attachment = new DefectAttachment
                    {
                        Id = Guid.NewGuid(),
                        FileName = $"evidencia_{i}.txt",
                        FileType = "text/plain",
                        UploadByUsername = assignee.FullName,
                        FileContent = System.Text.Encoding.UTF8.GetBytes(RemoveDiacritics("Evidência do erro...")),
                        CreatedAt = createdAt.AddHours(3)
                    };
                }

                // History (always include Create + a Resolved event to support UI projections)
                defect.DefectHistory.Add(new DefectChangeEvent
                {
                    Id = Guid.NewGuid(),
                    ContributorId = mainUser.Id,
                    Action = DefectAction.Create,
                    Field = null,
                    OldValue = null,
                    NewValue = null,
                    CreatedAt = createdAt
                });

                // Status changes
                var resolvedAt = createdAt.AddDays(rng.Next(1, 20));
                defect.DefectHistory.Add(new DefectChangeEvent
                {
                    Id = Guid.NewGuid(),
                    ContributorId = dev2.Id,
                    Action = DefectAction.Update,
                    Field = nameof(Defect.Status),
                    OldValue = DefectStatus.InProgress.ToString(),
                    NewValue = DefectStatus.Resolved.ToString(),
                    CreatedAt = resolvedAt
                });

                if (status == DefectStatus.Reopened)
                {
                    defect.DefectHistory.Add(new DefectChangeEvent
                    {
                        Id = Guid.NewGuid(),
                        ContributorId = tester.Id,
                        Action = DefectAction.Update,
                        Field = nameof(Defect.Status),
                        OldValue = DefectStatus.Resolved.ToString(),
                        NewValue = DefectStatus.Reopened.ToString(),
                        CreatedAt = resolvedAt.AddDays(2)
                    });
                }
                else if (status == DefectStatus.InProgress)
                {
                    defect.DefectHistory.Add(new DefectChangeEvent
                    {
                        Id = Guid.NewGuid(),
                        ContributorId = dev2.Id,
                        Action = DefectAction.Update,
                        Field = nameof(Defect.Status),
                        OldValue = DefectStatus.New.ToString(),
                        NewValue = DefectStatus.InProgress.ToString(),
                        CreatedAt = createdAt.AddDays(1)
                    });
                }

                // Trello linkage (some defects)
                if (i % 4 == 0)
                {
                    defect.TrelloUserStories.Add(new TrelloUserStory
                    {
                        Id = Guid.NewGuid(),
                        Name = $"US-{1000 + i} - {summaryText}",
                        Desc = "User story relacionada ao defeito",
                        ShortUrl = "https://trello.com/c/abcd1234"
                    });
                }

                // Mail Letter subscription (main user follows the defect)
                defect.ContributorMailLetter.Add(new DefectMailLetter
                {
                    ContributorId = mainUser.Id
                });

                createdDefects.Add(defect);
                db.Defects.Add(defect);
            }

            await db.SaveChangesAsync(cancellationToken);

            // Create some relations between defects (only within the same project)
            var all = await db.Defects.Where(d => d.ProjectId == project.Id).OrderBy(d => d.CreatedAt).ToListAsync(cancellationToken);
            for (int i = 0; i + 1 < all.Count && i < 6; i += 2)
            {
                var a = all[i];
                var b = all[i + 1];
                var already = await db.DefectRelations.AnyAsync(r => r.DefectId == a.Id && r.RelatedDefectId == b.Id, cancellationToken);
                if (!already)
                {
                    db.DefectRelations.Add(new DefectRelation { DefectId = a.Id, RelatedDefectId = b.Id });
                }
            }

            await db.SaveChangesAsync(cancellationToken);

            // Ensure coverage of all categories and statuses for ERP project only
            if (project.Name.Contains("ERP", StringComparison.OrdinalIgnoreCase))
            {
                // Ensure all categories
                foreach (var cat in Enum.GetValues<DefectCategory>())
                {
                    bool hasCat = await db.Defects.AnyAsync(d => d.ProjectId == project.Id && d.DefectCategory == cat, cancellationToken);
                    if (!hasCat)
                    {
                        await CreateCoverageDefect(project, mainUser, tester, dev2, rng,
                            summary: cat switch
                            {
                                DefectCategory.Functional => "ERP - Regra de negocio nao aplicada no faturamento",
                                DefectCategory.Interface => "ERP - Quebra de layout na tela de pedidos",
                                DefectCategory.Performance => "ERP - Lentidao ao processar fechamento do mes",
                                DefectCategory.Improvement => "ERP - Melhorar usabilidade no cadastro de clientes",
                                _ => "ERP - Defeito categorizado"
                            },
                            status: DefectStatus.InProgress,
                            category: cat,
                            cancellationToken);
                    }
                }

                // Ensure all statuses
                foreach (var st in Enum.GetValues<DefectStatus>())
                {
                    bool hasStatus = await db.Defects.AnyAsync(d => d.ProjectId == project.Id && d.Status == st, cancellationToken);
                    if (!hasStatus)
                    {
                        await CreateCoverageDefect(project, mainUser, tester, dev2, rng,
                            summary: st switch
                            {
                                DefectStatus.New => "ERP - Novo defeito reportado no modulo fiscal",
                                DefectStatus.InProgress => "ERP - Correcao em andamento no modulo de estoque",
                                DefectStatus.WaitingForUser => "ERP - Aguardando usuario anexar evidencias",
                                DefectStatus.Resolved => "ERP - Problema de comissao resolvido",
                                DefectStatus.Invalid => "ERP - Registro marcado como invalido",
                                DefectStatus.Reopened => "ERP - Defeito reaberto apos regressao",
                                _ => "ERP - Defeito com status"
                            },
                            status: st,
                            category: DefectCategory.Functional,
                            cancellationToken);
                    }
                }
            }

            return await db.Defects.Where(d => d.ProjectId == project.Id).ToListAsync(cancellationToken);
        }

        // Local function to create a single coverage defect with rich related data
        async Task CreateCoverageDefect(
            Project project,
            Contributor mainUser,
            Contributor tester,
            Contributor dev2,
            Random rng,
            string summary,
            DefectStatus status,
            DefectCategory category,
            CancellationToken cancellationToken)
        {
            var createdAt = project.CreatedAt.AddDays(rng.Next(10, 115)).AddHours(rng.Next(0, 23));
            var expiresIn = createdAt.AddDays(rng.Next(7, 45));
            var assignee = (rng.Next(0, 2) == 0) ? mainUser : dev2;

            var summaryText = RemoveDiacritics(summary);
            var descriptionText = RemoveDiacritics($"{summaryText} - Passos para reproduzir: ...");

            var defect = new Defect
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                AssignedToContributorId = assignee.Id,
                Summary = summaryText,
                Description = descriptionText,
                DefectCategory = category,
                DefectSeverity = Enum.GetValues<DefectSeverity>()[rng.Next(0, Enum.GetValues<DefectSeverity>().Length)],
                DefectEnvironment = Enum.GetValues<DefectEnvironment>()[rng.Next(0, Enum.GetValues<DefectEnvironment>().Length)],
                DefectPriority = Enum.GetValues<DefectPriority>()[rng.Next(0, Enum.GetValues<DefectPriority>().Length)],
                Version = project.Name.Contains("ERP") ? $"v1.{rng.Next(0, 4)}.{rng.Next(0, 9)}" : $"v2.{rng.Next(0, 2)}.{rng.Next(0, 9)}",
                ExpectedBehaviour = RemoveDiacritics("Processo deve executar conforme especificações"),
                ActualBehaviour = "Comportamento divergente observado",
                ErrorLog = rng.Next(0, 3) == 0 ? RemoveDiacritics("System.NullReferenceException em GestaoDefeitos.Core") : string.Empty,
                ExpiresIn = expiresIn,
                Status = status,
                CreatedAt = createdAt,
            };

            defect.Tags.Add(new Tag { Id = Guid.NewGuid(), Description = "coverage", CreatedAt = createdAt.AddMinutes(10) });

            defect.Comments.Add(new DefectComment
            {
                Id = Guid.NewGuid(),
                ContributorId = tester.Id,
                Content = RemoveDiacritics("Defeito criado para cobrir categoria/status ausente"),
                CreatedAt = createdAt.AddHours(1)
            });

            defect.DefectHistory.Add(new DefectChangeEvent
            {
                Id = Guid.NewGuid(),
                ContributorId = mainUser.Id,
                Action = DefectAction.Create,
                CreatedAt = createdAt
            });

            var resolvedAt = createdAt.AddDays(2);
            defect.DefectHistory.Add(new DefectChangeEvent
            {
                Id = Guid.NewGuid(),
                ContributorId = dev2.Id,
                Action = DefectAction.Update,
                Field = nameof(Defect.Status),
                OldValue = DefectStatus.InProgress.ToString(),
                NewValue = DefectStatus.Resolved.ToString(),
                CreatedAt = resolvedAt
            });

            db.Defects.Add(defect);
            await db.SaveChangesAsync(cancellationToken);
        }

        // Helper: ensure some explicit count for status in a project
        async Task EnsureSomeStatusAsync(Project project, DefectStatus status, int desired, string summaryPrefix)
        {
            var current = await db.Defects.CountAsync(d => d.ProjectId == project.Id && d.Status == status, cancellationToken);
            for (int i = current; i < desired; i++)
            {
                await CreateCoverageDefect(project, mainUser, tester, dev2, rng,
                    summary: RemoveDiacritics($"{summaryPrefix} #{i + 1}"),
                    status: status,
                    category: DefectCategory.Functional,
                    cancellationToken);
            }
        }

        // Admin project: 20+ defects
        await EnsureDefectsForProjectAsync(erp, 20);
        // Contributor project: a few defects (3)
        await EnsureDefectsForProjectAsync(ponto, 3);

        // Ensure explicit presence of NEW and RESOLVED in both projects
        await EnsureSomeStatusAsync(erp, DefectStatus.New, 5, "ERP - Novo defeito");
        await EnsureSomeStatusAsync(erp, DefectStatus.Resolved, 5, "ERP - Defeito resolvido");

        await EnsureSomeStatusAsync(ponto, DefectStatus.New, 2, "Ponto - Novo defeito");
        await EnsureSomeStatusAsync(ponto, DefectStatus.Resolved, 2, "Ponto - Defeito resolvido");

        // 6) Notifications (simple example)
        if (!await db.ContributorNotifications.AnyAsync(n => n.ContributorId == mainUser.Id, cancellationToken))
        {
            db.ContributorNotifications.Add(new ContributorNotification
            {
                Id = Guid.NewGuid(),
                ContributorId = mainUser.Id,
                Content = RemoveDiacritics("Bem-vindo! Seus projetos foram inicializados com dados de exemplo."),
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync(cancellationToken);
        }
    }

    private static string RemoveDiacritics(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
