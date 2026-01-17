using MudBlazor;
using MudBlazor.Utilities;

namespace PersonalPortfolio.Library.Domain;

public static class ThemeManager
{
    public static MudTheme GetMudTheme(WebsiteTheme websiteTheme)
    {
        var theme = new MudTheme();

        switch (websiteTheme)
        {
            case WebsiteTheme.Blue:
                theme = new MudTheme
                {
                    PaletteLight = new PaletteLight
                    {
                        AppbarBackground = "#0097FF",
                        AppbarText = "#FFFFFF",
                        Primary = "#007CD1",
                        PrimaryContrastText = "#FFFFFF",
                        TextPrimary = "#0C1217",
                        Background = "#F4FDFF",
                        TextSecondary = "#0C1217",
                        DrawerBackground = "#C5E5FF",
                        DrawerIcon = "#000000",
                        DrawerText = "#0C1217",
                        Surface = "#E4FAFF",
                        ActionDefault = "#0C1217",
                        ActionDisabled = "#2F678C",
                        TextDisabled = "#676767"
                    },
                    PaletteDark = new PaletteDark
                    {
                        AppbarBackground = "#0097FF",
                        AppbarText = "#FFFFFF",
                        Primary = "#007CD1",
                        PrimaryContrastText = "#FFFFFF",
                        Secondary = "#000000",
                        TextPrimary = "#FFFFFF",
                        Background = "#001524",
                        TextSecondary = "#E2EEF6",
                        DrawerBackground = "#093958",
                        DrawerIcon = "#FFFFFF",
                        DrawerText = "#FFFFFF",
                        Surface = "#093958",
                        ActionDefault = "#0C1217",
                        ActionDisabled = "#2F678C",
                        TextDisabled = "#B0B0B0"
                    }
                };
                break;

            case WebsiteTheme.Green:
                theme = new MudTheme
                {
                    //#00AA44
                    PaletteLight = new PaletteLight
                    {
                        AppbarBackground = "#00AA44",
                        Primary = new MudColor("#00D100"),
                        //Secondary = new MudColor("#000000")
                    },
                    PaletteDark = new PaletteDark
                    {
                        Primary =  new MudColor("#00D100"),
                        //Secondary =  new MudColor("#000000"
                    }
                };
                break;

            case WebsiteTheme.GreenCyan:
                theme = new MudTheme
                {
                    PaletteLight = new PaletteLight
                    {
                        Primary = "#78B7AD",
                        PrimaryContrastText = "#FFFFFF",
                        Secondary = "#5C9087",
                        AppbarBackground = "#78B7AD",
                        Background = "#FAFEFD",
                        Surface = "#FFFFFF",
                        DrawerBackground = "#FFFFFF",
                        TextPrimary = "#2F3B37",
                        TextSecondary = "#5C6B66",
                        ActionDefault = "#78B7AD",
                    },
                    PaletteDark = new PaletteDark
                    {
                        Primary = "#8AC7BD",
                        PrimaryContrastText = "#1A201E",
                        Secondary = "#78B7AD",
                        AppbarBackground = "#1A201E",
                        Background = "#121212",
                        Surface = "#1E1E1E",
                        DrawerBackground = "#1E1E1E",
                        TextPrimary = "#E0E0E0",
                        TextSecondary = "#B0B0B0",
                        ActionDefault = "#8AC7BD",
                    }
                };
                break;
        }

        return theme;
    }
}