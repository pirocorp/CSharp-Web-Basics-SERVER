namespace SIS.MvcFramework.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using Xunit;

    public class ViewEngineTests
    {
        [Theory]
        [InlineData("OnlyHtmlView")]
        [InlineData("LoopsView")]
        [InlineData("ViewModelView")]
        public void TestGetHtml(string testName)
        {
            var viewModel = new TestViewModel()
            {
                Name = "Niki",
                Year = 2020,
                Numbers = new List<int> {1, 10, 100, 1000, 10_000}
            };

            var viewContent = File.ReadAllText($"./ViewTests/{testName}.html");
            var expectedResultContent = File.ReadAllText($"./ViewTests/{testName}Expected.html");

            IViewEngine viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewContent, viewModel, "pirocorp");

            Assert.Equal(expectedResultContent, actualResult);
        }

        [Fact]
        public void TestHtmlWithCollectionForModel()
        {
            var viewModel = new List<int> {1, 2, 3};

            var viewContent = @"
@foreach (var num in Model)
{
<p>@num</p>
}";
            var expectedResultContent = @"<p>1</p>
<p>2</p>
<p>3</p>";

            IViewEngine viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewContent, viewModel, null);

            Assert.Equal(expectedResultContent, actualResult);
        }
    }
}
