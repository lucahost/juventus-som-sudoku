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

            sudokuFieldLoadedEventMock.Setup(
                 x =>
                 x.Subscribe(
                 It.IsAny<Action<List<Field>>>(),
                 It.IsAny<ThreadOption>(),
                 It.IsAny<bool>(),
                 It.IsAny<Predicate<List<Field>>>()))
                 .Callback<Action<List<Field>>, ThreadOption, bool, Predicate<List<Field>>>(
                 (e, t, b, p) => fieldsInitializedCallback = e);
        }


        private void GivenEmptyViewModel()
        {
            viewModel = new MainPageViewModel(navigationServiceMock.Object, eventAggregatorMock.Object, sudokuServiceMock.Object);
        }

        [TestMethod]
        public void MainPageViewModel_GivenEmptyViewModel_WhenReloadCommandIsClicked_ListIsInitialized()
        {
            GivenEmptyViewModel();

            viewModel.ReloadCommand.Execute();
            WhenFieldInitializedIsReceived(fields);

            Assert.IsNotNull(viewModel.Fields);
        }

        [TestMethod]
        public void MainPageViewModel_GivenNoEntryChanged_WhenValidateCommandIsClicked_TextChanges()
        {
            GivenEmptyViewModel();
            viewModel.ReloadCommand.Execute();
            WhenFieldInitializedIsReceived(fields);

            viewModel.ValidateCommand.Execute();

            Assert.AreNotEqual(viewModel.OutputText, "");
        }

        [TestMethod]
        public void MainPageViewModel_GivenNotCompleteFields_WhenValidateCommandIsClicked_TextChanges()
        {
            GivenEmptyViewModel();
            viewModel.ReloadCommand.Execute();
            fields[1].Value = -1;
            WhenFieldInitializedIsReceived(fields);

            viewModel.ValidateCommand.Execute();

            Assert.AreEqual(true, viewModel.OutputText.Contains("Nicht alle Felder"));
        }


        [TestMethod]
        public void MainPageViewModel_GivenIncorrectField_WhenValidateCommandIsClicked_TextChanges()
        {
            GivenEmptyViewModel();
            viewModel.ReloadCommand.Execute();
            fields[20].X = 10;
            fields[20].Value = 10;
            fields[21].X = 10;
            fields[21].Value = 10;
            WhenFieldInitializedIsReceived(fields);

            viewModel.ValidateCommand.Execute();

            Assert.AreEqual(true, viewModel.OutputText.Contains("Du hast eine ungültige"));
        }

        private void WhenFieldInitializedIsReceived(List<Field> fields)
        {
            fieldsInitializedCallback.Invoke(fields);
        }
    }
}
