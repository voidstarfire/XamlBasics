﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace XamlBasics
{
	public partial class HomePage
	{
		public HomePage()
		{
			InitializeComponent();

			//List of menu items.
			List<HomePageViewModel> models = new List<HomePageViewModel>
			{
				new HomePageViewModel(typeof(HelloXamlPage), NavigateTo, BrowseSource),
				//new HomePageViewModel(typeof(XamlPlusCodePage), NavigateTo, BrowseSource),
				new HomePageViewModel(typeof(GridDemoPage), NavigateTo, BrowseSource),
				//new HomePageViewModel(typeof(AbsoluteDemoPage), NavigateTo, BrowseSource),
				new HomePageViewModel(typeof(ResourceDictionary), NavigateTo, BrowseSource),
				new HomePageViewModel(typeof(StaticConstantsPage), NavigateTo, BrowseSource),
				new HomePageViewModel(typeof(RelativeLayoutPage), NavigateTo, BrowseSource),
				new HomePageViewModel(typeof(DataBindingsSliderRotation), NavigateTo, BrowseSource),
				new HomePageViewModel(typeof(DataBindingsFourSliders), NavigateTo, BrowseSource),
				new HomePageViewModel(typeof(DataBindingsListView), NavigateTo, BrowseSource),
				new HomePageViewModel(typeof(ClockViewModal), NavigateTo, BrowseSource),
				new HomePageViewModel(typeof(HSLColorScroll), NavigateTo, BrowseSource),
				new HomePageViewModel(typeof(KeypadPage), NavigateTo, BrowseSource)
			};

			listView.ItemsSource = models;
		}

		async void NavigateTo(Type pageType)
		{
			// Get all the constructors of the page type.
			IEnumerable<ConstructorInfo> constructors = pageType.GetTypeInfo().DeclaredConstructors;

			foreach (ConstructorInfo constructor in constructors)
			{
				// Check if the constructor has no parameters.
				if (constructor.GetParameters().Length == 0)
				{
					// If so, instantiate it, and navigate to it.
					Page page = (Page)constructor.Invoke(null);
					await this.Navigation.PushAsync(page);
					break;
				}
			}
		}

		async void BrowseSource(string pageName)
		{
			string xamlPage = "XamlSamples." + pageName + ".xaml";
			Assembly assembly = this.GetType().GetTypeInfo().Assembly;

			using (Stream stream = assembly.GetManifestResourceStream(xamlPage))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					string xaml = reader.ReadToEnd();
					await this.Navigation.PushAsync(new XamlBrowserPage(xaml));
				}
			}
		}

		// Also go to the page when the ListView item is selected.
		void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			HomePageViewModel viewModel = args.SelectedItem as HomePageViewModel;

			if (viewModel != null)
			{
				viewModel.GoToCommand.Execute(viewModel.PageType);
			}
		}
	}
}
