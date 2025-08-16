using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

/*
 * Playwright .NET Reference: https://playwright.dev/dotnet/docs/library
 * to take ss: await Page.ScreenshotAsync(new() { Path = "ss.png" });
 * 
 * This test suite does E2E UI testing for the Prometheus Group website using Playwright C# .NET NUnit
 * This test file is not refactored to use Page Object Model (POM) pattern
 * These tests have been refactored into the POM pattern under Pages/PrometheusGroupPage.cs and Tests/PrometheusGroupPageTests.cs
 */
namespace PlaywrightWebTests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class PrometheusWebTests : PageTest
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPage _page;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                //Headless = false,
                SlowMo = 800
            });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            if (_page != null) { await _page.CloseAsync(); } //close tab
            if (_context != null) { await _context.CloseAsync(); } //clear session
            if (_browser != null) { await _browser.CloseAsync(); } //close browser
            _playwright?.Dispose(); //dispose playwright
        }

        [Test]
        public async Task PrometheusLogoVisible()
        {
            await _page.GotoAsync("https://www.prometheusgroup.com/");
            //accept cookies to avoid blocking
            var acceptCookies = _page.Locator("#hs-eu-confirmation-button");
            if (await acceptCookies.IsVisibleAsync())
            {
                await acceptCookies.ClickAsync();
            }
            //close popup if it appears
            var closePopup = _page.Locator("#interactive-close-button");
            if (await closePopup.IsVisibleAsync())
            {
                await _page.ScreenshotAsync(new() { Path = "popup.png" });
                await closePopup.ClickAsync();
            }
            //escapes error message if it appears
            await _page.Keyboard.PressAsync("Escape");

            await CloseModalPopupAsync();

            //check that the logo is visible
            var logo = _page.Locator("img[src='https://www.prometheusgroup.com/hubfs/prometheus-group-srw/prometheus-group.svg']");
            await Expect(logo).ToBeVisibleAsync();
            await Expect(logo).ToHaveAttributeAsync("alt", "prometheus-group");
        }

        [Test]
        public async Task PrometheusGroupContactFormValidation()
        {
            await _page.GotoAsync("https://www.prometheusgroup.com/");

            //accept cookies to avoid blocking
            var acceptCookies = _page.Locator("#hs-eu-confirmation-button");
            if (await acceptCookies.IsVisibleAsync())
            {
                await acceptCookies.ClickAsync();
            }

            //close popup if it appears
            var closePopup = _page.Locator("#interactive-close-button");
            if (await closePopup.IsVisibleAsync())
            {
                await _page.ScreenshotAsync(new() { Path = "popup.png" });
                await closePopup.ClickAsync();
            }

            //escapes error message if it appears
            await _page.Keyboard.PressAsync("Escape");

            await CloseModalPopupAsync();

            //click Contact button
            await _page.Locator("#hs-cta-182765558177-0").ClickAsync(); 

            //find and fill first and last name fields
            var firstNameField = _page.Locator("#firstname-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            await firstNameField.FillAsync("Roy");

            var lastNameField = _page.Locator("#lastname-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            await lastNameField.FillAsync("G. Biv");

            //submit the form
            await _page.Locator("input[type='submit'][value='Contact Us']").ClickAsync();

            //validate that the form shows 4 errors for each missing field: "Please complete this required field."
            await Expect(_page.Locator("label.hs-error-msg.hs-main-font-element")).ToHaveCountAsync(4);
            await _page.ScreenshotAsync(new() { Path = "required_fields.png" });

            //validate that missing fields are empty: email, company, region, interested product
            var emailField = _page.Locator("#email-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            await Expect(emailField).ToHaveValueAsync(string.Empty);
            var companyField = _page.Locator("#company-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            await Expect(companyField).ToHaveValueAsync(string.Empty);
            var regionField = _page.Locator("#global_region-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            await Expect(regionField).ToHaveValueAsync(string.Empty);
            var productField = _page.Locator("#product-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            await Expect(productField).ToHaveValueAsync(string.Empty);
        }

        [Test]
        public async Task NavigateToAboutUsPage()
        {
            await _page.GotoAsync("https://www.prometheusgroup.com/");

            //accept cookies to avoid blocking
            var acceptCookies = _page.Locator("#hs-eu-confirmation-button");
            if (await acceptCookies.IsVisibleAsync())
            {
                await acceptCookies.ClickAsync();
            }

            //close popup if it appears
            var closePopup = _page.Locator("#interactive-close-button");
            if (await closePopup.IsVisibleAsync())
            {
                await _page.ScreenshotAsync(new() { Path = "popup.png" });
                await closePopup.ClickAsync();
            }

            //escapes error message if it appears
            await _page.Keyboard.PressAsync("Escape");

            await CloseModalPopupAsync();

            //hover over Company section in navbar and click About Us link
            await _page.Locator("span.parent-link.has-dropdown:has-text('Company')").HoverAsync();
            await _page.Locator("text='About Us'").First.ClickAsync();

            //validate that URL is correct and the header is visible
            await Expect(_page).ToHaveURLAsync(new Regex("https://www.prometheusgroup.com/company/about"));
            await Expect(_page.Locator("h2:has-text('The Global Leader in EAM')")).ToBeVisibleAsync();
        }

        public async Task CloseModalPopupAsync()
        {
            var overlay = _page.Locator("#hs-interactives-modal-overlay");
            var closeTop = _page.Locator("#interactive-close-button, #interactive-close-button-container");

            if (await closeTop.CountAsync() > 0 && await closeTop.IsVisibleAsync())
                await closeTop.ClickAsync(new() { Force = true });

            await _page.Keyboard.PressAsync("Escape");

            //try closing from inside the HubSpot iframe
            var popupFrame = _page.FrameLocator(
                "iframe[role='dialog'][title='Popup CTA'], " +
                "iframe[data-test-id='interactive-frame'], " +
                "iframe[src*='hs-web-interactive']"
            );
            var closeInFrame = popupFrame.Locator("#interactive-close-button, [aria-label='Close'], button:has-text('Close')");
            if (await closeInFrame.CountAsync() > 0 && await closeInFrame.IsVisibleAsync())
                await closeInFrame.ClickAsync(new() { Force = true });
        }
    }
}
