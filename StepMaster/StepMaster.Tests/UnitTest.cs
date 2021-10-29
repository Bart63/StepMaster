using StepMaster.ViewModels;
using Xunit;

namespace StepMaster.Tests
{
    public class UnitTest
    {
        [Fact]
        public void TestViewModel()
        {
            var itemsViewModel = new StatisticsViewModel();
            itemsViewModel.OnAppearing();
            Assert.True(itemsViewModel.IsBusy);
        }
    }
}
