using System.Linq;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using MudBlazor.Services;
using PersonalPortfolio.Library.Domain;
using PersonalPortfolio.Library.Infrastructure;
using Xunit;

namespace EventInvitations.Tests.Blazor;

public class WeddingInvitationTests : BunitContext
{
    private static WebsiteData BuildWebsiteData(
        string? hero = "hero.jpg",
        string couple = "Alex & Rocío",
        string date = "June 1, 2026",
        string subtitle = "Join us",
        string scroll = "Scroll down",
        string footer = "Gracias")
    {
        return new WebsiteData
        {
            WeddingInvitation = new WeddingInvitation
            {
                HeroBackgroundImage = hero,
                CoupleNames = couple,
                DateText = date,
                Subtitle = subtitle,
                ScrollIndicatorText = scroll,
                FooterText = footer,
                Sections = new()
                {
                    new WeddingSection { Title = "Our Story", Image = "story.jpg", ImageAlt = "story", Paragraphs = new() { "p1", "p2" } },
                    new WeddingSection { Title = "Venue", Reverse = true, Image = "venue.jpg", ImageAlt = "venue", Paragraphs = null! }
                }
            }
        };
    }

    private void RegisterCommonServices(WebsiteData website)
    {
        Services.AddMudServices();

        var repo = new Mock<IWebsiteRepo>();
        repo.Setup(x => x.GetWebsiteData()).ReturnsAsync(website);
        Services.AddSingleton(repo.Object);

        // JSInterop expectations for OnAfterRenderAsync
        JSInterop.SetupVoid("weddingSnow.init").SetVoidResult();
        JSInterop.SetupVoid("scrollFade.init").SetVoidResult();
        JSInterop.SetupVoid("backgroundMusic.init", "music/wedding-background-music.mp3").SetVoidResult();
    }

    [Fact]
    public void Renders_Hero_And_Basic_Text_From_WebsiteRepo()
    {
        // Arrange
        var data = BuildWebsiteData();
        RegisterCommonServices(data);

        // Act
        var cut = Render<PersonalPortfolio.Blazor.Pages.WeddingInvitation>();

        // Assert
        cut.Find(".couple").TextContent.Should().Be("Alex & Rocío");
        cut.Find(".date").TextContent.Should().Be("June 1, 2026");
        cut.Find(".subtitle").TextContent.Should().Be("Join us");
        cut.Find(".scroll-indicator").TextContent.Should().Be("Scroll down");
        cut.Find(".footer").TextContent.Should().Be("Gracias");

        // Hero style should include background only when non-empty
        var hero = cut.Find(".hero");
        hero.GetAttribute("style").Should().Contain("background-image:url('hero.jpg')");
    }

    [Fact]
    public void Renders_Sections_With_Reverse_And_Paragraphs_Safely()
    {
        // Arrange
        var data = BuildWebsiteData();
        RegisterCommonServices(data);

        // Act
        var cut = Render<PersonalPortfolio.Blazor.Pages.WeddingInvitation>();

        // Assert
        var sections = cut.FindAll(".fade-section");
        sections.Should().HaveCount(2);

        // Section 1: not reversed => image then text
        var inner1 = sections[0].GetElementsByTagName("div").First(e => e.ClassList.Contains("section-inner"));
        var children1 = inner1.Children.ToArray();
        children1[0].ClassName.Should().Contain("section-image");
        children1[1].ClassName.Should().Contain("section-text");
        children1[1].TextContent.Should().Contain("Our Story");
        children1[1].TextContent.Should().Contain("p1");

        // Section 2: reversed => text then image, and paragraphs null handled gracefully
        var inner2 = sections[1].GetElementsByTagName("div").First(e => e.ClassList.Contains("section-inner"));
        inner2.ClassList.Contains("reverse").Should().BeTrue();
        var children2 = inner2.Children.ToArray();
        children2[0].ClassName.Should().Contain("section-text");
        children2[1].ClassName.Should().Contain("section-image");
    }

    [Fact]
    public void JSInterop_Is_Called_Only_On_First_Render()
    {
        // Arrange
        var data = BuildWebsiteData();
        RegisterCommonServices(data);

        // Act
        var cut = Render<PersonalPortfolio.Blazor.Pages.WeddingInvitation>();
        cut.Render(); // trigger a subsequent render; OnAfterRenderAsync(firstRender=false)

        // Assert - allow async OnAfterRenderAsync to complete
        cut.WaitForAssertion(() =>
        {
            JSInterop.Invocations.Count(i => i.Identifier == "weddingSnow.init").Should().Be(1);
            JSInterop.Invocations.Count(i => i.Identifier == "backgroundMusic.init").Should().Be(1);
        }, timeout: System.TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void When_No_Hero_Image_Then_No_Background_Style_Is_Applied()
    {
        // Arrange
        var data = BuildWebsiteData(hero: " \t\n");
        RegisterCommonServices(data);

        // Act
        var cut = Render<PersonalPortfolio.Blazor.Pages.WeddingInvitation>();

        // Assert
        var hero = cut.Find(".hero");
        hero.GetAttribute("style")?.Should().NotContain("background-image");
    }

    [Fact]
    public void Renders_Events_Section_With_Target_Blank_On_Links()
    {
        // Arrange
        var data = BuildWebsiteData();
        data.WeddingInvitation.Sections.Add(new WeddingSection
        {
            Type = WeddingSectionType.Events,
            Title = "Events",
            Events = new()
            {
                new WeddingEvent
                {
                    Title = "Ceremony",
                    LocationUrl = "https://maps.google.com"
                }
            }
        });
        RegisterCommonServices(data);

        // Act
        var cut = Render<PersonalPortfolio.Blazor.Pages.WeddingInvitation>();

        // Assert
        var link = cut.Find(".event-link");
        link.GetAttribute("target").Should().Be("_blank");
    }

    [Fact]
    public void Renders_DressCode_Section_With_Note()
    {
        // Arrange
        var data = BuildWebsiteData();
        data.WeddingInvitation.Sections.Add(new WeddingSection
        {
            Type = WeddingSectionType.DressCode,
            Title = "Vestimenta",
            Image = "dress.png",
            FooterText = "Favor de reservar colores blanco, rosa y fucsia."
        });
        RegisterCommonServices(data);

        // Act
        var cut = Render<PersonalPortfolio.Blazor.Pages.WeddingInvitation>();

        // Assert
        var dressNote = cut.Find(".dress-note");
        dressNote.TextContent.Should().Be("Favor de reservar colores blanco, rosa y fucsia.");
    }

    [Fact]
    public void Renders_Photos_Section_With_Action_Button()
    {
        // Arrange
        var data = BuildWebsiteData();
        data.WeddingInvitation.Sections.Add(new WeddingSection
        {
            Type = WeddingSectionType.Photos,
            Title = "Fotos",
            ActionUrl = "https://photos.amazon.com/xyz",
            ActionText = "Subir Fotos"
        });
        RegisterCommonServices(data);

        // Act
        var cut = Render<PersonalPortfolio.Blazor.Pages.WeddingInvitation>();

        // Assert
        var actionButton = cut.Find(".action-container a");
        actionButton.GetAttribute("href").Should().Be("https://photos.amazon.com/xyz");
        actionButton.TextContent.Should().Contain("Subir Fotos");
        actionButton.GetAttribute("target").Should().Be("_blank");
    }

    [Fact]
    public void Renders_Photos_Section_With_Animated_Camera_And_QR_Code()
    {
        // Arrange
        var data = BuildWebsiteData();
        data.WeddingInvitation.Sections.Add(new WeddingSection
        {
            Type = WeddingSectionType.Photos,
            Title = "Fotos",
            ActionUrl = "https://photos.amazon.com/xyz"
        });
        RegisterCommonServices(data);

        // Act
        var cut = Render<PersonalPortfolio.Blazor.Pages.WeddingInvitation>();

        // Assert
        cut.Find(".camera-container").Should().NotBeNull();
        cut.Find(".camera-svg").Should().NotBeNull();
        cut.Find(".camera-path").Should().NotBeNull();

        var qrCodeImage = cut.Find(".qr-code-image");
        qrCodeImage.GetAttribute("src").Should().Contain("api.qrserver.com");
        qrCodeImage.GetAttribute("src").Should().Contain("https%3A%2F%2Fphotos.amazon.com%2Fxyz");
    }

    [Fact]
    public void Does_Not_Render_QR_Code_When_Disabled()
    {
        // Arrange
        var data = BuildWebsiteData();
        data.WeddingInvitation.Sections.Add(new WeddingSection
        {
            Type = WeddingSectionType.Photos,
            Title = "Fotos",
            ActionUrl = "https://photos.amazon.com/xyz",
            ShowQrCode = false
        });
        RegisterCommonServices(data);

        // Act
        var cut = Render<PersonalPortfolio.Blazor.Pages.WeddingInvitation>();

        // Assert
        cut.FindAll(".qr-code-image").Should().BeEmpty();
    }
}