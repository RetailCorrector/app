namespace RetailCorrector;

public static class Env
{
    public static Report Report { get; set; } = new Report();
    public static Guid WorkspaceId
    {
        get => _workspaceId;
        set
        {
            _workspaceId = value;
        }
    }
    private static Guid _workspaceId = Guid.CreateVersion7();
}
