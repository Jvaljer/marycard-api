using System.Reflection;

namespace VideoTag;

public static class Module
{
    public static readonly Assembly[] RegisteredModules = [
        VideoModule.Reference.Assembly,
        AuthModule.Reference.Assembly,
        JobModule.Reference.Assembly,
        InventoryModule.Reference.Assembly,
        AnalyticsModule.Reference.Assembly,
        OrderModule.Reference.Assembly,
    ];
}