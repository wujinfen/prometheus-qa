using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using PlaywrightWebTests.Pages;

namespace PlaywrightWebTests.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class PrometheusGroupTests : PageTest
    {
        [Test]
        public async Task PrometheusLogoVisible()
        {
            var prometheusPage = new PrometheusGroupPage(Page);
            await prometheusPage.GoToAsync();
            await prometheusPage.CloseAllPopupsAsync();
            await prometheusPage.CloseModalPopupAsync();
            await prometheusPage.CheckPgLogoAsync();
        }

        [Test]
        public async Task PrometheusGroupContactFormRequiredFields()
        {
            var prometheusPage = new PrometheusGroupPage(Page);
            await prometheusPage.GoToAsync();
            await prometheusPage.CloseAllPopupsAsync();
            await prometheusPage.CloseModalPopupAsync();
            await Page.ScreenshotAsync(new() { Path = "page.png", FullPage = true });

            await prometheusPage.ClickContactButtonAsync();
            await prometheusPage.FillFirstAndLastNameAsync();
            await prometheusPage.SubmitContactFormAsync();
            await prometheusPage.ValidateContactFormAsync();
        }

        [Test]
        public async Task PrometheusGroupNavigateToAboutUsPage()
        {
            var prometheusPage = new PrometheusGroupPage(Page);
            await prometheusPage.GoToAsync();
            await prometheusPage.CloseAllPopupsAsync();
            await prometheusPage.CloseModalPopupAsync();
            await prometheusPage.NavigateToAboutUsPageAsync();
            await prometheusPage.ValidateAboutPageURLandHeaderAsync();
        }
    }
}
