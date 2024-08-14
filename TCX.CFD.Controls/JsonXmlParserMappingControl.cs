using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class JsonXmlParserMappingControl : UserControl
{
	private IContainer components;

	private PictureBox deletePicture;

	private Label lblPath;

	private Label lblVariable;

	private ComboBox comboVariables;

	private TextBox txtPath;

	public string Path => txtPath.Text;

	public string Variable
	{
		get
		{
			if (comboVariables.SelectedIndex != -1)
			{
				return comboVariables.SelectedItem.ToString();
			}
			return "";
		}
	}

	private void ResizeDropDown()
	{
		string text = string.Empty;
		foreach (string ıtem in comboVariables.Items)
		{
			if (ıtem.Length > text.Length)
			{
				text = ıtem;
			}
		}
		SizeF sizeF = CreateGraphics().MeasureString(text, comboVariables.Font);
		comboVariables.DropDownWidth = Math.Max(comboVariables.Width, Convert.ToInt32(sizeF.Width));
	}

	public JsonXmlParserMappingControl(ResponseMapping responseMapping, List<string> variables)
	{
		InitializeComponent();
		lblPath.Text = LocalizedResourceMgr.GetString("JsonXmlParserMappingControl.lblPath.Text");
		lblVariable.Text = LocalizedResourceMgr.GetString("JsonXmlParserMappingControl.lblVariable.Text");
		txtPath.Text = responseMapping.Path;
		ComboBox.ObjectCollection ıtems = comboVariables.Items;
		object[] items = variables.ToArray();
		ıtems.AddRange(items);
		comboVariables.Items.Add(LocalizedResourceMgr.GetString("JsonXmlParserMappingControl.comboVariables.Create"));
		comboVariables.SelectedIndex = variables.IndexOf(responseMapping.Variable);
		ResizeDropDown();
	}

	private void TxtPath_Enter(object sender, EventArgs e)
	{
		txtPath.SelectAll();
	}

	private void ComboVariables_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (comboVariables.SelectedIndex == comboVariables.Items.Count - 1)
		{
			string value = (base.Parent?.Parent?.Parent as JsonXmlParserWizardPage2)?.CreateVariable();
			comboVariables.SelectedIndex = (string.IsNullOrEmpty(value) ? (-1) : comboVariables.Items.IndexOf(value));
		}
	}

	private void DeletePicture_Click(object sender, EventArgs e)
	{
		(base.Parent?.Parent?.Parent as JsonXmlParserWizardPage2)?.DeleteResponseMapping(this);
	}

	public void AddCreatedVariable(string variable)
	{
		comboVariables.Items.Insert(comboVariables.Items.Count - 1, variable);
		ResizeDropDown();
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
		this.deletePicture = new System.Windows.Forms.PictureBox();
		this.lblPath = new System.Windows.Forms.Label();
		this.lblVariable = new System.Windows.Forms.Label();
		this.comboVariables = new System.Windows.Forms.ComboBox();
		this.txtPath = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.deletePicture).BeginInit();
		base.SuspendLayout();
		this.deletePicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.deletePicture.Image = TCX.CFD.Properties.Resources.Edit_Delete;
		this.deletePicture.Location = new System.Drawing.Point(376, 23);
		this.deletePicture.Name = "deletePicture";
		this.deletePicture.Size = new System.Drawing.Size(24, 24);
		this.deletePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.deletePicture.TabIndex = 1;
		this.deletePicture.TabStop = false;
		this.deletePicture.Click += new System.EventHandler(DeletePicture_Click);
		this.lblPath.AutoSize = true;
		this.lblPath.Location = new System.Drawing.Point(-3, 3);
		this.lblPath.Name = "lblPath";
		this.lblPath.Size = new System.Drawing.Size(37, 17);
		this.lblPath.TabIndex = 0;
		this.lblPath.Text = "Path";
		this.lblVariable.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblVariable.AutoSize = true;
		this.lblVariable.Location = new System.Drawing.Point(205, 3);
		this.lblVariable.Name = "lblVariable";
		this.lblVariable.Size = new System.Drawing.Size(60, 17);
		this.lblVariable.TabIndex = 2;
		this.lblVariable.Text = "Variable";
		this.comboVariables.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.comboVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboVariables.FormattingEnabled = true;
		this.comboVariables.Location = new System.Drawing.Point(208, 23);
		this.comboVariables.Name = "comboVariables";
		this.comboVariables.Size = new System.Drawing.Size(162, 24);
		this.comboVariables.TabIndex = 3;
		this.comboVariables.SelectedIndexChanged += new System.EventHandler(ComboVariables_SelectedIndexChanged);
		this.txtPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPath.Location = new System.Drawing.Point(0, 23);
		this.txtPath.Name = "txtPath";
		this.txtPath.Size = new System.Drawing.Size(202, 22);
		this.txtPath.TabIndex = 1;
		this.txtPath.Enter += new System.EventHandler(TxtPath_Enter);
		this.AllowDrop = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.txtPath);
		base.Controls.Add(this.comboVariables);
		base.Controls.Add(this.lblVariable);
		base.Controls.Add(this.lblPath);
		base.Controls.Add(this.deletePicture);
		base.Name = "JsonXmlParserMappingControl";
		base.Size = new System.Drawing.Size(400, 55);
		((System.ComponentModel.ISupportInitialize)this.deletePicture).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
