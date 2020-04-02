namespace LogBuckets.Shared
{
    internal interface INavigation
    {
        bool NavigateTo();
        bool NavigateFrom();
    }
}
