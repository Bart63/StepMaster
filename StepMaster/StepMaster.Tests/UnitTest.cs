using StepMaster.ViewModels;
using Xunit;

namespace StepMaster.Tests
{
    public class UnitTest
    {
        [Fact]
        public void TestViewModel()
        {
            var itemsViewModel = new ItemsViewModel();
            itemsViewModel.OnAppearing();
            Assert.True(itemsViewModel.IsBusy);
        }
    }
}
