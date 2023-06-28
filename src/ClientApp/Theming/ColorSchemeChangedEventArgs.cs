﻿namespace BlazorApp1.Theming;

public class ColorSchemeChangedEventArgs : EventArgs
{
    public ColorSchemeChangedEventArgs(ColorScheme colorScheme)
    {
        ColorScheme = colorScheme;
    }

    public ColorScheme ColorScheme { get; }
}