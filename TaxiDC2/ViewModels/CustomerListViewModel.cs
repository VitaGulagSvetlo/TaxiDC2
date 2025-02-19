﻿using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TaxiDC2.ViewModels
{
    public class CustomerListViewModel : BaseViewModel
    {
        private IApiProxy _proxy;

        private Customer _selectedItem;
        private string _filter="A";

        public ObservableCollection<Customer> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Customer> ItemTapped { get; }

        public CustomerListViewModel(IApiProxy proxy)
        {
            _proxy = proxy;
            Title = "Zákazníci";
            Items = new ObservableCollection<Customer>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Customer>(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                await RefreshData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshData()
        {
            Items.Clear();
            var result = await _proxy.FindCustomersAsync(_filter);
            IEnumerable<Customer> d = result.Data.ToList();

            if (result.State == ResultCode.OK)
            {
                foreach (Customer item in d)
                {
                    Items.Add(item);
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Customer SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(DetailZakaznik));
        }

        async void OnItemSelected(Customer item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(DetailZakaznik)}?id={item.IdCustomer}");
        }

        public async Task SaveToggleData()
        {
            try
            {
                foreach (Customer customer in Items.Where(w => w.IsDirty))
                {
                    ServiceResult ret = await _proxy.UpdateCustomerSetings(customer);
                    if (ret.State == ResultCode.OK)
                        customer.IsDirty = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void Search(string te)
        {
            _filter = te;
            RefreshData();
        }
    }
}