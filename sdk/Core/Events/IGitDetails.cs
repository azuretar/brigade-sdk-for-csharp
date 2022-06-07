namespace Brigade.Core.Events;

/**
 * Represents git-specific Event details. These may override Project-level
 * GitConfig.
 */
public interface IGitDetails
{
    /**
	 * Specifies the location from where a source code repository may be cloned
	 */
    string? CloneURL { get; set; }
    /**
	 * Specifies a commit (by sha) to be checked out. If specified, takes
	 * precedence over any tag or branch specified by the ref field.
	 */
    string? Commit { get; set; }
    /**
	 * Specifies a tag or branch to be checked out. If left blank, this will
	 * default to "master" at runtime.
	 */
    string? Reference { get; set; }
}

public class GitDetails : IGitDetails
{
    public string? CloneURL { get; set; }
    public string? Commit { get; set; }
    public string? Reference { get; set; }
}