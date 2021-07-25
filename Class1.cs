using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EntryTest
{
    [TestFixture]
    public class Class1
    {
        IWebDriver Browser;
        WebDriverWait Wait;

        string userEmail = "zhaklinak@gmail.com";
        string userPassword = "test123";

        [SetUp]
        public void SetUp()
        {
            Browser = new ChromeDriver();

            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Browser.Manage().Window.Maximize();
            Browser.Navigate().GoToUrl("http://automationpractice.com/index.php");

            IWebElement SignInButton = Browser.FindElement(By.ClassName("login"));
            SignInButton.Click();
        }

        [Test]
        public void Part1()
        {
            bool RegistrationForm = Browser.FindElement(By.Id("create-account_form")).Displayed;
            Assert.IsTrue(RegistrationForm);

            IWebElement RegisterEmail = Browser.FindElement(By.Id("email_create"));
            RegisterEmail.SendKeys(userEmail);

            IWebElement SubmitEmail = Browser.FindElement(By.Id("SubmitCreate"));
            SubmitEmail.Click();

            Wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(3000));

            IWebElement RegFirstName = Browser.FindElement(By.Id("customer_firstname"));
            RegFirstName.SendKeys("Zhaklin");

            IWebElement RegLastName = Browser.FindElement(By.Id("customer_lastname"));
            RegLastName.SendKeys("Delev");

            IWebElement Password = Browser.FindElement(By.ClassName("password"));
            IWebElement RegPassword = Password.FindElement(By.Id("passwd"));
            RegPassword.SendKeys(userPassword);

            IWebElement formText = Browser.FindElement(By.ClassName("form_info"));
            formText.Click();

            Assert.IsTrue(Password.GetAttribute("class").Contains("form-ok"));

            IWebElement AddressFirstName = Browser.FindElement(By.Id("firstname"));
            Assert.AreEqual("Zhaklin", AddressFirstName.GetAttribute("value"));

            IWebElement AddressLastName = Browser.FindElement(By.Id("lastname"));
            Assert.AreEqual("Delev", AddressLastName.GetAttribute("value"));

            IWebElement Address1 = Browser.FindElement(By.Id("address1"));
            Address1.SendKeys("dummy address");

            IWebElement city = Browser.FindElement(By.Id("city"));
            city.SendKeys("New York");

            IWebElement state = Browser.FindElement(By.Id("id_state"));
            var selectState = new SelectElement(state);
            selectState.SelectByText("New York");

            IWebElement zipCode = Browser.FindElement(By.Id("postcode"));
            zipCode.SendKeys("34567");

            IWebElement country = Browser.FindElement(By.Id("id_country"));
            var selectCountry = new SelectElement(country);
            selectCountry.SelectByText("United States");

            IWebElement mobilePhone = Browser.FindElement(By.Id("phone_mobile"));
            mobilePhone.SendKeys("123456789");

            IWebElement aliasAddress = Browser.FindElement(By.Id("alias"));
            aliasAddress.Clear();
            aliasAddress.SendKeys(userEmail);

            IWebElement submitAccountReg = Browser.FindElement(By.Id("submitAccount"));
            submitAccountReg.Click();

            string expectedMyAccountUrl = "http://automationpractice.com/index.php?controller=my-account";
            string actualMyAccountUrl = Browser.Url;
            Assert.AreEqual(expectedMyAccountUrl, actualMyAccountUrl);
        }

        [Test]
        public void Part2()
        {
            IWebElement logInEmail = Browser.FindElement(By.Id("email"));
            logInEmail.SendKeys(userEmail);

            IWebElement logInPassword = Browser.FindElement(By.Id("passwd"));
            logInPassword.SendKeys(userPassword);

            IWebElement submitLogin = Browser.FindElement(By.Id("SubmitLogin"));
            submitLogin.Click();

            IWebElement home = Browser.FindElement(By.ClassName("icon-home"));
            home.Click();

            IWebElement topMenu = Browser.FindElement(By.Id("block_top_menu"));
            Assert.IsTrue(topMenu.Displayed);

            IWebElement dressessCategory = topMenu.FindElement(By.CssSelector("ul.menu-content>li:nth-child(2)"));
            IWebElement dressAnchor = dressessCategory.FindElement(By.CssSelector("a[title=\"Dresses\"]"));

            Assert.IsTrue(dressAnchor.Displayed);

            Actions hover = new Actions(Browser);
            hover.MoveToElement(dressAnchor).Perform();

            IWebElement summerDresses = dressessCategory.FindElement(By.CssSelector("ul.submenu-container li:nth-child(3) a[title=\"Summer Dresses\"]"));
            Assert.IsTrue(summerDresses.Displayed);

            summerDresses.Click();

            List<IWebElement> discountedDresses = Browser.FindElements(By.CssSelector("#center_column div.right-block div[itemprop='offers'] .price-percent-reduction")).ToList();
            Assert.AreEqual(2, discountedDresses.Count);

            List<IWebElement> allSummerDresses = Browser.FindElements(By.CssSelector("#center_column ul.product_list > li")).ToList();

            foreach (IWebElement dressNotDiscounted in allSummerDresses)
            {
                if (dressNotDiscounted.FindElements(By.CssSelector(" div.right-block div[itemprop='offers'] .price-percent-reduction")).Count() == 0)
                {
                    Actions hover2 = new Actions(Browser);
                    hover2.MoveToElement(dressNotDiscounted).Perform();
                    dressNotDiscounted.FindElement(By.CssSelector(".addToWishlist")).Click();
                    break;
                }
            }

            IWebElement confirmationDialog = Browser.FindElement(By.CssSelector(".fancybox-wrap"));
            Assert.True(confirmationDialog.Displayed);
            IWebElement confirmationText = confirmationDialog.FindElement(By.CssSelector("p.fancybox-error"));
            Assert.True(confirmationText.Displayed);
            Assert.AreEqual("Added to your wishlist.", confirmationText.Text);
        }

        [Test]
        public void Part3()
        {
            IWebElement logInEmail = Browser.FindElement(By.Id("email"));
            logInEmail.SendKeys(userEmail);

            IWebElement logInPassword = Browser.FindElement(By.Id("passwd"));
            logInPassword.SendKeys(userPassword);

            IWebElement submitLogin = Browser.FindElement(By.Id("SubmitLogin"));
            submitLogin.Click();

            IWebElement home = Browser.FindElement(By.ClassName("icon-home"));
            home.Click();

            List<IWebElement> homeClothing = Browser.FindElements(By.CssSelector("#homefeatured li .product-name")).ToList();
            homeClothing.Count();

            Random randomClothing = new Random();
            int selectRandomClothing = randomClothing.Next(0, (homeClothing.Count));
            string productNameRandom = homeClothing[selectRandomClothing].Text;
            homeClothing[selectRandomClothing].Click();


            IWebElement size = Browser.FindElement(By.Id("group_1"));
            var selectSize = new SelectElement(size);
            selectSize.SelectByText("M");


            IWebElement price = Browser.FindElement(By.Id("our_price_display"));
            string priceText = price.Text;

            IWebElement addToCart = Browser.FindElement(By.Id("add_to_cart"));
            addToCart.Click();

            IWebElement continueShopping = Browser.FindElement(By.CssSelector("#layer_cart .clearfix .layer_cart_cart .button-container .continue"));
            continueShopping.Click();

            IWebElement shoppingCart = Browser.FindElement(By.CssSelector("#header .container .row .clearfix .shopping_cart a[title='View my shopping cart']"));

            // 6

            Actions hover3 = new Actions(Browser);
            hover3.MoveToElement(shoppingCart).Perform();

            IWebElement productPrice = Browser.FindElement(By.CssSelector("#header .cart-info .price"));
            var innerHtmlPP = productPrice.GetAttribute("innerHTML");
            Assert.AreEqual(priceText, innerHtmlPP);

            IWebElement totalPrice = Browser.FindElement(By.CssSelector("#header .container .row .clearfix .shopping_cart .cart_block .cart-prices .last-line .price"));
            var innerHtmlTP = totalPrice.GetAttribute("innerHTML");

            // 7

            IWebElement checkoutButton = Browser.FindElement(By.Id("button_order_cart"));
            checkoutButton.Click();

            // 8

            IWebElement productName = Browser.FindElement(By.CssSelector("#cart_summary .cart_item .cart_description .product-name"));
            Assert.AreEqual(productNameRandom, productName.Text);

            IWebElement productPriceInCheckout = Browser.FindElement(By.Id("total_price"));
            Assert.AreEqual(innerHtmlTP, productPriceInCheckout.Text);

            // 9

            IWebElement proceedToCheckout = Browser.FindElement(By.CssSelector("#center_column .button"));
            proceedToCheckout.Click();

            // 10

            IWebElement verifyAddress = Browser.FindElement(By.Id("id_address_delivery"));
            Assert.AreEqual(userEmail, verifyAddress.Text);

            // 11 

            IWebElement proceedtoCheckoutAddress = Browser.FindElement(By.CssSelector("#center_column button[type='submit']"));
            proceedtoCheckoutAddress.Click();

            // 12

            IWebElement checkbox = Browser.FindElement(By.Id("cgv"));
            checkbox.Click();

            // 13

            IWebElement proceedToCheckoutShipping = Browser.FindElement(By.CssSelector("#form .cart_navigation button[type='submit']"));
            proceedToCheckoutShipping.Click();

            // 14

            IWebElement totalPricePayment = Browser.FindElement(By.Id("total_price"));
            Assert.AreEqual(innerHtmlTP, totalPricePayment.Text);


            // 15

            IWebElement bankwirePayment = Browser.FindElement(By.CssSelector("#HOOK_PAYMENT .bankwire"));
            bankwirePayment.Click();

            // 16

            IWebElement confirmOrder = Browser.FindElement(By.CssSelector("#center_column .cart_navigation .button"));
            confirmOrder.Click();

            // 17

            IWebElement orderConfirmation = Browser.FindElement(By.CssSelector("#order-confirmation .center_column .cheque-indent"));
            StringAssert.Contains("Your order on My Store is complete.", orderConfirmation.Text);
        }

        [TearDown]
        public void Teardown()
        {
            Browser.Close();
        }

    }
}
