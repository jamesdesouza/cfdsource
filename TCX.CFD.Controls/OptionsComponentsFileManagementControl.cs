using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsFileManagementControl : UserControl, IOptionsControl
{
	private IContainer components;

	private Label lblAction;

	private ComboBox comboOpenMode;

	private Label lblOpenMode;

	private ComboBox comboAction;

	private ErrorProvider errorProvider;

	private Label lblReadWriteToFile;

	private FileManagementOpenModes GetOpenMode(string str)
	{
		return str switch
		{
			"Append" => FileManagementOpenModes.Append, 
			"Create" => FileManagementOpenModes.Create, 
			"CreateNew" => FileManagementOpenModes.CreateNew, 
			"Open" => FileManagementOpenModes.Open, 
			"OpenOrCreate" => FileManagementOpenModes.OpenOrCreate, 
			"Truncate" => FileManagementOpenModes.Truncate, 
			_ => FileManagementOpenModes.Open, 
		};
	}

	private string GetOpenModeAsStr(FileManagementOpenModes openMode)
	{
		return openMode switch
		{
			FileManagementOpenModes.Append => "Append", 
			FileManagementOpenModes.Create => "Create", 
			FileManagementOpenModes.CreateNew => "CreateNew", 
			FileManagementOpenModes.Open => "Open", 
			FileManagementOpenModes.OpenOrCreate => "OpenOrCreate", 
			FileManagementOpenModes.Truncate => "Truncate", 
			_ => "Open", 
		};
	}

	private void ValidateOpenMode()
	{
		if ((FileManagementActions)comboAction.SelectedItem == FileManagementActions.Read && (FileManagementOpenModes)comboOpenMode.SelectedItem != FileManagementOpenModes.Open && (FileManagementOpenModes)comboOpenMode.SelectedItem != FileManagementOpenModes.OpenOrCreate)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsFileManagementControl.Error.InvalidOpenMode"));
		}
	}

	private void ValidateFields()
	{
		ValidateOpenMode();
	}

	private void Combo_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateOpenMode();
			errorProvider.SetError(comboOpenMode, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(comboOpenMode, ErrorHelper.GetErrorDescription(exc));
		}
	}

	public OptionsComponentsFileManagementControl()
	{
		InitializeComponent();
		comboOpenMode.Items.AddRange(new object[6]
		{
			FileManagementOpenModes.Append,
			FileManagementOpenModes.Create,
			FileManagementOpenModes.CreateNew,
			FileManagementOpenModes.Open,
			FileManagementOpenModes.OpenOrCreate,
			FileManagementOpenModes.Truncate
		});
		comboAction.Items.AddRange(new object[2]
		{
			FileManagementActions.Read,
			FileManagementActions.Write
		});
		comboOpenMode.SelectedItem = GetOpenMode(Settings.Default.FileManagementTemplateOpenMode);
		comboAction.SelectedItem = ((!(Settings.Default.FileManagementTemplateAction == "Read")) ? FileManagementActions.Write : FileManagementActions.Read);
		Combo_Validating(comboOpenMode, new CancelEventArgs());
		Combo_Validating(comboAction, new CancelEventArgs());
		lblReadWriteToFile.Text = LocalizedResourceMgr.GetString("OptionsComponentsFileManagementControl.lblReadWriteToFile.Text");
		lblOpenMode.Text = LocalizedResourceMgr.GetString("OptionsComponentsFileManagementControl.lblOpenMode.Text");
		lblAction.Text = LocalizedResourceMgr.GetString("OptionsComponentsFileManagementControl.lblAction.Text");
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.FileManagementTemplateOpenMode = GetOpenModeAsStr((FileManagementOpenModes)comboOpenMode.SelectedItem);
		Settings.Default.FileManagementTemplateAction = (((FileManagementActions)comboAction.SelectedItem == FileManagementActions.Read) ? "Read" : "Write");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		this.lblAction = new System.Windows.Forms.Label();
		this.comboOpenMode = new System.Windows.Forms.ComboBox();
		this.lblOpenMode = new System.Windows.Forms.Label();
		this.comboAction = new System.Windows.Forms.ComboBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblReadWriteToFile = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblAction.AutoSize = true;
		this.lblAction.Location = new System.Drawing.Point(9, 87);
		this.lblAction.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblAction.Name = "lblAction";
		this.lblAction.Size = new System.Drawing.Size(47, 17);
		this.lblAction.TabIndex = 3;
		this.lblAction.Text = "Action";
		this.comboOpenMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboOpenMode.FormattingEnabled = true;
		this.comboOpenMode.Location = new System.Drawing.Point(12, 59);
		this.comboOpenMode.Margin = new System.Windows.Forms.Padding(4);
		this.comboOpenMode.Name = "comboOpenMode";
		this.comboOpenMode.Size = new System.Drawing.Size(300, 24);
		this.comboOpenMode.TabIndex = 2;
		this.comboOpenMode.Validating += new System.ComponentModel.CancelEventHandler(Combo_Validating);
		this.lblOpenMode.AutoSize = true;
		this.lblOpenMode.Location = new System.Drawing.Point(9, 38);
		this.lblOpenMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblOpenMode.Name = "lblOpenMode";
		this.lblOpenMode.Size = new System.Drawing.Size(82, 17);
		this.lblOpenMode.TabIndex = 1;
		this.lblOpenMode.Text = "Open Mode";
		this.comboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboAction.FormattingEnabled = true;
		this.comboAction.Location = new System.Drawing.Point(12, 108);
		this.comboAction.Margin = new System.Windows.Forms.Padding(4);
		this.comboAction.Name = "comboAction";
		this.comboAction.Size = new System.Drawing.Size(300, 24);
		this.comboAction.TabIndex = 4;
		this.comboAction.Validating += new System.ComponentModel.CancelEventHandler(Combo_Validating);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblReadWriteToFile.AutoSize = true;
		this.lblReadWriteToFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblReadWriteToFile.Location = new System.Drawing.Point(8, 8);
		this.lblReadWriteToFile.Name = "lblReadWriteToFile";
		this.lblReadWriteToFile.Size = new System.Drawing.Size(174, 20);
		this.lblReadWriteToFile.TabIndex = 0;
		this.lblReadWriteToFile.Text = "Read / Write to File";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblReadWriteToFile);
		base.Controls.Add(this.comboAction);
		base.Controls.Add(this.lblAction);
		base.Controls.Add(this.comboOpenMode);
		base.Controls.Add(this.lblOpenMode);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsFileManagementControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
