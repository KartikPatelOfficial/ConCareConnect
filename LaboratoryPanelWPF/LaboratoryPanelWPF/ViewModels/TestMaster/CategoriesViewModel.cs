using System;
using LaboratoryPanelWPF.Model;
using LaboratoryPanelWPF.Service;
using LaboratoryPanelWPF.Views.Dialog;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui;
using CommunityToolkit.Mvvm.Input;
using LaboratoryPanelWPF.Views.Pages.TestMaster;

namespace LaboratoryPanelWPF.ViewModels.TestMaster
{
    public class CategoriesViewModel : FormViewModel
    {
        private readonly IDBService _dbService;
        private readonly INavigationService _navigationService;


        public ICommand PreviousPageCommand => new RelayCommand(PreviousPage_Click);
        public ICommand NextPageCommand => new RelayCommand(NextPage_Click);
        
        public int[] PageInfo => new[] { _currentPage, _totalPages };


        private int _currentPage = 1;
        private readonly int _itemsPerPage = 10;
        private readonly int _totalItems;
        private readonly int _totalPages;


        private ObservableCollection<Category> Categories { get; }
        public ObservableCollection<Category> PagedCategories { get; set; }

        public ICommand RunAddCategoryDialogCommand { get; set; }
        public ICommand RunEditCategoryDialogCommand { get; set; }
        public ICommand RunDeleteCategoryDialogCommand { get; set; }

        public ICommand RunViewCategoryDialogCommand { get; set; }

        public CategoriesViewModel(IDBService dbService, INavigationService navigationService)
        {
            _dbService = dbService;
            _navigationService = navigationService;
            Categories = new ObservableCollection<Category>(_dbService.GetCategories());
            _totalItems = Categories.Count;
            _totalPages = (int)Math.Ceiling((decimal)_totalItems / _itemsPerPage);
            PagedCategories = new ObservableCollection<Category>();
            UpdateDataGrid();

            RunAddCategoryDialogCommand = new Utils.RelayCommand<Window>(RunAddCategoryDialog);
            RunEditCategoryDialogCommand = new Utils.RelayCommand<Category>(RunEditCategoryDialog);
            RunDeleteCategoryDialogCommand = new Utils.RelayCommand<Category>(RunDeleteCategoryDialog);
            RunViewCategoryDialogCommand = new Utils.RelayCommand<Category>(RunViewCategoryDialog);
        }

        private void UpdateDataGrid()
        {
            var skip = (_currentPage - 1) * _itemsPerPage;
            var take = Math.Min(_itemsPerPage, _totalItems - skip);

            PagedCategories.Clear();
            foreach (var category in Categories.Skip(skip).Take(take))
            {
                PagedCategories.Add(category);
            }
            
            OnPropertyChanged(nameof(PageInfo));
            
        }

        private void PreviousPage_Click()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                UpdateDataGrid();
            }
        }

        private void NextPage_Click()
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                UpdateDataGrid();
            }
        }

        private void RunViewCategoryDialog(Category category)
        {
            var dataContext = new TestGroupInCategoryViewModel(_dbService,category);
            _navigationService.NavigateWithHierarchy(typeof(TestGroupInCategoryPage), dataContext);
        }

        private void RunDeleteCategoryDialog(Category category)
        {
            var messageBox = MessageBox.Show("Are you sure you want to delete this category?", "Delete Category",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBox != MessageBoxResult.Yes) return;
            _dbService.DeleteCategory(category.Id);
            Categories.Remove(category);
        }

        private void RunEditCategoryDialog(Category category)
        {
            var dialog = new CategoryDialog();
            dialog.DataContext = new CategoryDialogViewModel(DialogMode.Edit)
            {
                Name = category.Name,
                PrintingName = category.PrintingName,
                Cost = category.Cost,
                LabCharges = category.LabCharge,
                Expenses = category.Expenses
            };
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                category.Name = ((CategoryDialogViewModel)dialog.DataContext).Name;
                category.PrintingName = ((CategoryDialogViewModel)dialog.DataContext).PrintingName;
                category.Cost = ((CategoryDialogViewModel)dialog.DataContext).Cost;
                category.LabCharge = ((CategoryDialogViewModel)dialog.DataContext).LabCharges;
                category.Expenses = ((CategoryDialogViewModel)dialog.DataContext).Expenses;

                _dbService.EditCategory(category);
            }
        }

        private void RunAddCategoryDialog(Window window)
        {
            var dialog = new CategoryDialog();
            dialog.DataContext = new CategoryDialogViewModel(DialogMode.Add);
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                var category = new Category()
                {
                    Name = ((CategoryDialogViewModel)dialog.DataContext).Name,
                    PrintingName = ((CategoryDialogViewModel)dialog.DataContext).PrintingName,
                    Cost = ((CategoryDialogViewModel)dialog.DataContext).Cost,
                    LabCharge = ((CategoryDialogViewModel)dialog.DataContext).LabCharges,
                    Expenses = ((CategoryDialogViewModel)dialog.DataContext).Expenses
                };

                category = _dbService.AddCategory(category);
                Categories.Add(category);
            }
        }
    }
}