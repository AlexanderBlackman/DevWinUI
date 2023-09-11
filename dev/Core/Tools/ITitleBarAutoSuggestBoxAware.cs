﻿namespace WinUICommunity.Core.Tools;
public interface ITitleBarAutoSuggestBoxAware
{
    void OnAutoSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args);
    void OnAutoSuggestBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args);
}
