﻿namespace DevWinUI;
internal partial class CalendarViewAttach
{
    public static readonly DependencyProperty ShowBorderProperty =
    DependencyProperty.RegisterAttached(
        "ShowBorder",
        typeof(bool),
        typeof(CalendarViewAttach),
        new PropertyMetadata(false, OnShowBorderChanged));

    public static bool GetShowBorder(DependencyObject obj) =>
        (bool)obj.GetValue(ShowBorderProperty);

    public static void SetShowBorder(DependencyObject obj, bool value) =>
        obj.SetValue(ShowBorderProperty, value);

    private static void OnShowBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CalendarView calendarView && calendarView != null)
        {
            FrameworkElement headerBorder = DependencyObjectEx.FindDescendant(calendarView, "PART_HeaderBorder");
            FrameworkElement topBorder = DependencyObjectEx.FindDescendant(calendarView, "PART_TopBorder");
            var headerButton = DependencyObjectEx.FindDescendant(calendarView, "HeaderButton") as Button;
            var nextButton = DependencyObjectEx.FindDescendant(calendarView, "NextButton") as Button;
            var previousButton = DependencyObjectEx.FindDescendant(calendarView, "PreviousButton") as Button;

            var showBorder = (bool)e.NewValue;
            calendarView.Loaded -= CalendarView_Loaded;
            calendarView.Loaded += CalendarView_Loaded;

            void CalendarView_Loaded(object sender, RoutedEventArgs e)
            {
                if (headerBorder == null || topBorder == null || headerButton == null || nextButton == null || previousButton == null)
                {
                    headerBorder = DependencyObjectEx.FindDescendant(calendarView, "PART_HeaderBorder");
                    topBorder = DependencyObjectEx.FindDescendant(calendarView, "PART_TopBorder");
                    headerButton = DependencyObjectEx.FindDescendant(calendarView, "HeaderButton") as Button;
                    nextButton = DependencyObjectEx.FindDescendant(calendarView, "NextButton") as Button;
                    previousButton = DependencyObjectEx.FindDescendant(calendarView, "PreviousButton") as Button;

                    UpdateBorders();
                }
            }

            void UpdateBorders()
            {
                if (headerBorder != null)
                {
                    headerBorder.Visibility = showBorder ? Visibility.Visible : Visibility.Collapsed;
                }
                if (topBorder != null)
                {
                    if (showBorder)
                    {
                        topBorder.Height = 0;
                    }
                    else
                    {
                        topBorder.Height = 1;
                    }
                }

                if (headerButton == null || nextButton == null || previousButton == null)
                {
                    return;
                }

                if (showBorder)
                {
                    headerButton.Style = Application.Current.Resources["AccentHeaderNavigationButtonStyle"] as Style;
                    nextButton.Style = Application.Current.Resources["AccentNavigationButtonStyle"] as Style;
                    previousButton.Style = Application.Current.Resources["AccentNavigationButtonStyle"] as Style;
                }
                else
                {
                    headerButton.Style = Application.Current.Resources["HeaderNavigationButtonStyle"] as Style;
                    nextButton.Style = Application.Current.Resources["NavigationButtonStyle"] as Style;
                    previousButton.Style = Application.Current.Resources["NavigationButtonStyle"] as Style;
                }
            }
            UpdateBorders();
        }
    }
}
