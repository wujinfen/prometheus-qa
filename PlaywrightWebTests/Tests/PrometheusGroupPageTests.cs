using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightWebTests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Playwright.Assertions;
using static System.Net.Mime.MediaTypeNames;

namespace PlaywrightWebTests.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class PrometheusGroupTests : PageTest
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
                SlowMo = 400
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
