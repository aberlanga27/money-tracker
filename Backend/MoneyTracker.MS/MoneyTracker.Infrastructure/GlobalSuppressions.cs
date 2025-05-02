using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Protected readonly field for DI", Scope = "member", Target = "~F:MoneyTracker.Infrastructure.Repositories.Repository`1.context")]
[assembly: SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for validation of DI app settings singleton", Scope = "member", Target = "~F:MoneyTracker.Infrastructure.Providers.LocalizationProvider.appSettings")]