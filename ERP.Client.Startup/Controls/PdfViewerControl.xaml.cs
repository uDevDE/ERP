﻿using ERP.Client.Controls;
using ERP.Client.Core.Enums;
using ERP.Client.Model;
using ERP.Client.ViewModel.PdfViewer;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ERP.Client.Startup.Controls
{
    public sealed partial class PdfViewerControl : UserControl
    {
        public delegate void ButtonCloseClickedHandler(PdfViewerControl pdfViewer);
        public event ButtonCloseClickedHandler ButtonCloseClicked;

        public delegate void ButtonFavoriteToggledHandler(PdfViewerControl pdfViewer, PdfFileModel pdfFile);
        public event ButtonFavoriteToggledHandler ButtonFavoriteToggled;

        private bool ignoreEvent;

        public PdfViewerViewModel ViewerPageViewModel { get; set; }
        public StorageFile File { get; private set; }
        public PdfFileModel PdfFile { get; set; }
        public bool PdfLoaded { get; private set; }
        public PdfViewerViewType PdfViewerViewType { get; private set; }
        public bool? IsMainPdfFile { get; private set; }
        public bool PageLoaded { get; private set; }

        private static System.Collections.Generic.List<string> _loadedFiles = new System.Collections.Generic.List<string>();

        public PdfViewerControl(StorageFile file, PdfFileModel pdfFile, bool isMain)
        {
            this.InitializeComponent();

            ViewerPageViewModel = new PdfViewerViewModel();
            PdfViewerViewType = PdfViewerViewType.Normal;

            File = file;
            PdfFile = pdfFile;
            IsMainPdfFile = isMain;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsMainPdfFile == true)
            {
                ButtonClose.IsEnabled = false;
            }

            if (File != null && !PageLoaded && !_loadedFiles.Contains(File.Path))
            {
                _loadedFiles.Add(File.Path);
                var pdfDocument = await PdfDocument.LoadFromFileAsync(File);
                var pageCount = pdfDocument.PageCount;
                NumberBoxPageNumber.Maximum = pageCount;
                PageLoaded = true;

                if (pageCount == 1)
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { LoadingControl.IsLoading = true; });
                }

                for (uint i = 0; i < pdfDocument.PageCount; i++)
                {
                    var page = pdfDocument.GetPage(i);
                    if (page != null)
                    {
                        var pdfPageControl = new PdfPageControl();
                        await pdfPageControl.Load(page);
                        ViewerPageViewModel.Pages.Add(pdfPageControl);
                    }
                }

                if (pageCount == 1)
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { LoadingControl.IsLoading = false; });
                }
                PdfLoaded = true;
            }
        }

        private void GoToPage(uint pageIndex)
        {
            var page = ViewerPageViewModel?.GetPage(pageIndex);
            if (page != null)
            {
                page.StartBringIntoView(new BringIntoViewOptions() { AnimationDesired = true });
                PdfViewScrollViewer.UpdateLayout();
            }
        }

        private void PdfViewScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                var zoomFactor = scrollViewer.ZoomFactor;
                if (zoomFactor < 0.2f && PdfViewerViewType == PdfViewerViewType.Normal)
                {

                }

                var page = ViewerPageViewModel?.GetCurrentPage();
                if (page != null)
                {
                    ignoreEvent = true;
                    NumberBoxPageNumber.Value = Convert.ToDouble(page.Page.Index + 1);
                    ignoreEvent = false;
                }

            }
        }

        private void ButtonScrollToFirst_Click(object sender, RoutedEventArgs e) =>
            PdfViewScrollViewer.ChangeView(PdfViewScrollViewer.HorizontalOffset, 0, PdfViewScrollViewer.ZoomFactor);

        private void ButtonScrollToLast_Click(object sender, RoutedEventArgs e)
        {
            var page = ViewerPageViewModel.GetLastPage();
            if (page != null)
            {
                page.StartBringIntoView();
                PdfViewScrollViewer.UpdateLayout();
            }
        }

        private void NumberBoxPageNumber_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
        {
            if (ignoreEvent)
                return;

            GoToPage(Convert.ToUInt32(args.NewValue));
        }

        private void ButtonRotateLeft_Click(object sender, RoutedEventArgs e)
        {
            var page = ViewerPageViewModel.CurrentPage;
            if (page != null)
            {
                page.RotateLeft();
                PdfViewScrollViewer.UpdateLayout();
            }
        }

        private void ButtonRotateRight_Click(object sender, RoutedEventArgs e)
        {
            var page = ViewerPageViewModel.CurrentPage;
            if (page != null)
            {
                page.RotateRight();
                PdfViewScrollViewer.UpdateLayout();
            }
        }

        private void ButtonGoBack_Click(object sender, RoutedEventArgs e)
        {
            var pageIndex = Convert.ToUInt32(NumberBoxPageNumber.Value);
            if (pageIndex > 1)
            {
                var newPageIndex = pageIndex - 1;
                GoToPage(newPageIndex);
            }
        }

        private void ButtonGoNext_Click(object sender, RoutedEventArgs e)
        {
            var pageIndex = Convert.ToUInt32(NumberBoxPageNumber.Value);
            if (pageIndex < ViewerPageViewModel.Pages.Count)
            {
                var newPageIndex = pageIndex + 1;
                GoToPage(newPageIndex);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e) => ButtonCloseClicked?.Invoke(this);

        private void ButtonFavorite_Checked(object sender, RoutedEventArgs e)
        {
            PdfFile.IsFavorite = true;
            ButtonFavoriteToggled?.Invoke(this, PdfFile);
        }

        private void ButtonFavorite_Unchecked(object sender, RoutedEventArgs e)
        {
            PdfFile.IsFavorite = false;
            ButtonFavoriteToggled?.Invoke(this, PdfFile);
        }
    }
}
