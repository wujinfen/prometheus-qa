using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightWebTests.Pages
{
    public class PrometheusGroupPage
    {
        private readonly IPage _page;
        private ILocator AcceptCookies;
        private ILocator ClosePopup;
        private ILocator PgLogo;
        private ILocator ContactButton;
        private ILocator FirstNameField;
        private ILocator LastNameField;
        private ILocator SubmitContactButton;
        private ILocator MissingFieldRequirementError;
        private ILocator EmailField;
        private ILocator CompanyField;
        private ILocator RegionField;
        private ILocator ProductField;
        private ILocator CompanyNavbar;
        private ILocator AboutUsLink;


        public PrometheusGroupPage(IPage page)
        {
            _page = page;
            AcceptCookies = _page.Locator("#hs-eu-confirmation-button");
            ClosePopup = _page.Locator("#interactive-close-button");
            PgLogo = _page.Locator("img[src='https://www.prometheusgroup.com/hubfs/prometheus-group-srw/prometheus-group.svg']");
            ContactButton = _page.Locator("#hs-cta-182765558177-0");
            FirstNameField = _page.Locator("#firstname-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            LastNameField = _page.Locator("#lastname-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            SubmitContactButton = _page.Locator("input[type='submit'][value='Contact Us']");
            MissingFieldRequirementError = _page.Locator("label.hs-error-msg.hs-main-font-element");
            EmailField = _page.Locator("#email-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            CompanyField = _page.Locator("#company-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            RegionField = _page.Locator("#global_region-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            ProductField = _page.Locator("#product-fe70f03d-5bac-4ad3-a698-3e130182d674_8630");
            CompanyNavbar = _page.Locator("span.parent-link.has-dropdown:has-text('Company')");
            AboutUsLink = _page.Locator("text='About Us'");
        }

        //goes to Prometheus Group website
        public async Task GoToAsync()
        {
            await _page.GotoAsync("https://www.prometheusgroup.com/");
        }

        public async Task CloseAllPopupsAsync()
        {
            //accept cookies to avoid blocking
            if (await AcceptCookies.IsVisibleAsync())
            {
                await AcceptCookies.ClickAsync();
            }
            //close popup if it appears
            if (await ClosePopup.IsVisibleAsync())
            {
                await ClosePopup.ClickAsync();
            }
            //escapes error message if it appears
            await _page.Keyboard.PressAsync("Escape");
        }

        public async Task CloseModalPopupAsync()
        {
            var overlay = _page.Locator("#hs-interactives-modal-overlay");
            var closeTop = _page.Locator("#interactive-close-button, #interactive-close-button-container");

            if (await closeTop.CountAsync() > 0 && await closeTop.IsVisibleAsync())
                await closeTop.ClickAsync(new() { Force = true });

            await _page.Keyboard.PressAsync("Escape");
        }

        public async Task CheckPgLogoAsync()
        {
            await Expect(PgLogo).ToBeVisibleAsync();
            await Expect(PgLogo).ToHaveAttributeAsync("alt", "prometheus-group");
        }

        public async Task ClickContactButtonAsync()
        {
            await ContactButton.ClickAsync();
        }

        public async Task FillFirstAndLastNameAsync()
        {
            await FirstNameField.FillAsync("Roy");
            await LastNameField.FillAsync("G.Biv");
        }

        public async Task SubmitContactFormAsync()
        {
            await SubmitContactButton.ClickAsync();
        }

        public async Task ValidateContactFormAsync()
        {
            //validate that the form has 4 errors for each missing field:   
            await Expect(MissingFieldRequirementError).ToHaveCountAsync(4);

            //validate that the missing fields are empty
            await Expect(EmailField).ToHaveValueAsync(string.Empty);
            await Expect(CompanyField).ToHaveValueAsync(string.Empty);
            await Expect(RegionField).ToHaveValueAsync(string.Empty);
            await Expect(ProductField).ToHaveValueAsync(string.Empty);
        }

        public async Task NavigateToAboutUsPageAsync()
        {
            await CompanyNavbar.HoverAsync();
            await AboutUsLink.First.ClickAsync();
        }

        public async Task ValidateAboutPageURLandHeaderAsync()
        {
            await Expect(_page).ToHaveURLAsync(new Regex("https://www.prometheusgroup.com/company/about"));
            await Expect(_page.Locator("h2:has-text('The Global Leader in EAM')")).ToBeVisibleAsync();
        }
    }
}

