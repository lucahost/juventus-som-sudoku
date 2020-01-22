using Microsoft.VisualStudio.TestTools.UnitTesting;
using Juventus.SOM.Sudoku.ViewModels;
using System.Collections.Generic;
using Juventus.SOM.Sudoku.Models;
using System;
using Prism.Events;
using Moq;

namespace Juventus.SOM.Sudoku.Test
{
    [TestClass]
    public class MainPageViewModelTest : TestBase
    {
        MainPageViewModel viewModel;

        Action<List<Field>> fieldsInitializedCallback;
        [TestInitialize]
        public void TestInitialize()
        {
            TestSetup();
        }


        private void GivenEmptyViewModel()
        {
            viewModel = new MainPageViewModel(navigationServiceMock.Object, eventAggregatorMock.Object, sudokuServiceMock.Object);
        }

        [TestMethod]
        public void MainPageViewModel_GivenEmptyViewModel_WhenListIsLoaded_ListIsInitialized()
        {
            sudokuFieldLoadedEventMock.Setup(
             x =>
             x.Subscribe(
             It.IsAny<Action<List<Field>>>(),
             It.IsAny<ThreadOption>(),
             It.IsAny<bool>(),
             It.IsAny<Predicate<List<Field>>>()))
             .Callback<Action<List<Field>>, ThreadOption, bool, Predicate<List<Field>>>(
             (e, t, b, p) => fieldsInitializedCallback = e);


            GivenEmptyViewModel();

            viewModel.ReloadCommand.Execute();

            WhenFieldInitializedIsReceived(fields);

            Assert.IsNotNull(viewModel.Fields);
        }

        private void WhenFieldInitializedIsReceived(List<Field> fields)
        {
            fieldsInitializedCallback.Invoke(fields);
        }

    }
}
