# HSLTravelSharp
A .NET Standard C# libray for reading data from an HSL Travel Card. A C# port of the [HSL Travel Card Java SDK](https://github.com/hsldevcom/hsl-card-java).

The library is divided into two parts: Core and UWP.

## HslTravelSharp.Core

This library can be used as a dependency for any C# library that might implement a platform-specific HSL travel card reader.

Contains all the types and logic for transforming bytes on the card into meaningful data structures. The two primary types you'll be dealing with here are the [TravelCard](https://github.com/pingzing/hsl-travel-sharp/blob/master/HslTravelSharp.Core/Models/TravelCard.cs) and [ETicket](https://github.com/pingzing/hsl-travel-sharp/blob/master/HslTravelSharp.Core/Models/ETicket.cs) classes. Each of these are proxy classes that wrap up a [RawTravelCard](https://github.com/pingzing/hsl-travel-sharp/blob/master/HslTravelSharp.Core/Models/RawTravelCard.cs) and [RawETicket](https://github.com/pingzing/hsl-travel-sharp/blob/master/HslTravelSharp.Core/Models/RawETicket.cs) class respectively. The "Raw" classes contain mostly uninterpreted byte values, whereas the wrapper classes have made an attempt to turn those byte values into something more meaningful.

## HslTravelSharp.UWP

Contains an example implementation of constructing a TravelCard instance using UWP's SmartCard classes. Depends on HslTravelSharp.Core.

# Usage

Both libraries are available on NuGet.

## Core

Use this if you want to implement a travel card reader for another platform.

[https://www.nuget.org/packages/HslTravelSharp.Core/](https://www.nuget.org/packages/HslTravelSharp.Core/)

## UWP

Use this for to build a UWP app that can read travel cards.

[https://www.nuget.org/packages/HslTravelSharp.Uwp/](https://www.nuget.org/packages/HslTravelSharp.Uwp/)

Example (Note: this example requires the "Windows Mobile Extensions for the UWP" extension package):

```C#
using HslTravelSharp.Core.Models;
using HslTravelSharpUwp;
using System;
using Windows.Devices.Enumeration;
using Windows.Devices.SmartCards;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Page declaration boilerplate removed for brevity...

 private async void MainPage_Loaded(object sender, RoutedEventArgs e)
 {
     // Find an NFC reader
     string selector = SmartCardReader.GetDeviceSelector();
     DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);
 
     foreach (DeviceInformation device in devices)
     {
         SmartCardReader reader = await SmartCardReader.FromIdAsync(device.Id);
         reader.CardAdded += Reader_CardAdded;
         reader.CardRemoved += Reader_CardRemoved;
     }           
 }
 
 private async void Reader_CardAdded(SmartCardReader sender, CardAddedEventArgs args)
 {
     // Obtain your travel card here
     TravelCard card = await CardOperations.ReadTravelCardAsync(args.SmartCard);

     // Update your UI, etc
 }

 private void Reader_CardRemoved(SmartCardReader sender, CardRemovedEventArgs args)
 {
     // Update your UI, etc.
 }
```