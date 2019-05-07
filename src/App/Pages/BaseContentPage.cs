﻿using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bit.App.Pages
{
    public class BaseContentPage : ContentPage
    {
        protected int AndroidShowModalAnimationDelay = 400;
        protected int AndroidShowPageAnimationDelay = 100;

        protected void SetActivityIndicator()
        {
            Content = new ActivityIndicator
            {
                IsRunning = true,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };
        }

        protected async Task LoadOnAppearedAsync(View viewToSet, bool fromModal, Func<Task> workFunction)
        {
            async Task DoWorkAsync()
            {
                await workFunction.Invoke();
                if(viewToSet != null)
                {
                    Content = viewToSet;
                }
            }
            if(Device.RuntimePlatform == Device.iOS)
            {
                await DoWorkAsync();
                return;
            }
            await Task.Run(async () =>
            {
                await Task.Delay(fromModal ? AndroidShowModalAnimationDelay : AndroidShowPageAnimationDelay);
                Device.BeginInvokeOnMainThread(async () => await DoWorkAsync());
            });
        }

        protected void RequestFocus(InputView input)
        {
            if(Device.RuntimePlatform == Device.iOS)
            {
                input.Focus();
                return;
            }
            Task.Run(async () =>
            {
                await Task.Delay(AndroidShowModalAnimationDelay);
                Device.BeginInvokeOnMainThread(() => input.Focus());
            });
        }
    }
}
