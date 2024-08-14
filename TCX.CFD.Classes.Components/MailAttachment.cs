using System.ComponentModel;
using System.Drawing.Design;
using TCX.CFD.Classes.Editors;

namespace TCX.CFD.Classes.Components;

public class MailAttachment
{
	public delegate void NameChangingEventHandler(object sender, NameChangingEventArgs e);

	public delegate void NameChangedEventHandler(object sender, NameChangedEventArgs e);

	private IVadActivity containerActivity;

	private string attachmentName;

	private string attachmentFile;

	public string Name
	{
		get
		{
			return attachmentName;
		}
		set
		{
			if (!(value != attachmentName))
			{
				return;
			}
			NameChangingEventArgs nameChangingEventArgs = new NameChangingEventArgs(attachmentName, value);
			if (this.NameChanging != null)
			{
				this.NameChanging(this, nameChangingEventArgs);
			}
			if (!nameChangingEventArgs.Cancel)
			{
				string oldValue = attachmentName;
				attachmentName = value;
				if (this.NameChanged != null)
				{
					this.NameChanged(this, new NameChangedEventArgs(oldValue, attachmentName));
				}
			}
		}
	}

	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string File
	{
		get
		{
			return attachmentFile;
		}
		set
		{
			attachmentFile = value;
		}
	}

	public event NameChangingEventHandler NameChanging;

	public event NameChangedEventHandler NameChanged;

	public MailAttachment()
	{
		attachmentName = string.Empty;
		attachmentFile = string.Empty;
	}

	public MailAttachment(string attachmentName, string attachmentFile)
	{
		this.attachmentName = attachmentName;
		this.attachmentFile = attachmentFile;
	}

	public IVadActivity GetContainerActivity()
	{
		return containerActivity;
	}

	public void SetContainerActivity(IVadActivity containerActivity)
	{
		this.containerActivity = containerActivity;
	}

	public bool IsNameChangingHandled()
	{
		return this.NameChanging != null;
	}

	public bool IsNameChangedHandled()
	{
		return this.NameChanged != null;
	}
}
