
/// <summary>
/// Possible states of an asynchronous operation.
/// </summary> 
public enum BindingAsyncState
{
None,
Running,
Cancelling,
Cancelled,
Succeeded,
Failed
}