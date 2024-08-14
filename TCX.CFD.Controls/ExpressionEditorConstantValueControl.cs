using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class ExpressionEditorConstantValueControl : UserControl, IExpressionEditorControl
{
	private ConstantValueTypes forcedType;

	private IContainer components;

	private TextBox txtConstantValue;

	private ComboBox comboBooleanValues;

	private Label lblConstantValue;

	private PictureBox menuPicture;

	private ContextMenuStrip contextMenu;

	private ToolStripMenuItem deleteMenuOption;

	private ToolStripMenuItem changeTypeMenuOption;

	private ToolStripMenuItem stringMenuOption;

	private ToolStripMenuItem multilineStringMenuOption;

	private ToolStripMenuItem singleCharacterMenuOption;

	private ToolStripMenuItem booleanMenuOption;

	private ToolStripMenuItem integerNumberMenuOption;

	private ToolStripMenuItem floatingPointNumberMenuOption;

	public ConstantValueTypes ForcedType
	{
		set
		{
			if (value != 0)
			{
				forcedType = value;
				comboBooleanValues.Visible = forcedType == ConstantValueTypes.Boolean;
				txtConstantValue.Visible = forcedType != ConstantValueTypes.Boolean;
				if (forcedType == ConstantValueTypes.MultilineString)
				{
					txtConstantValue.Multiline = true;
					txtConstantValue.Height = 50;
					txtConstantValue.AcceptsReturn = true;
					txtConstantValue.ScrollBars = ScrollBars.Vertical;
					base.Height = 78;
				}
				else
				{
					txtConstantValue.Multiline = true;
					txtConstantValue.Height = 22;
					txtConstantValue.AcceptsReturn = false;
					txtConstantValue.ScrollBars = ScrollBars.None;
					base.Height = 50;
				}
				(base.Parent?.Parent as ExpressionEditorRowControl)?.UpdateRowHeight(base.Height);
				lblConstantValue.Text = LocalizedResourceMgr.GetString("ExpressionEditorConstantValueControl.lblConstantValue." + GetForcedTypeAsString() + ".Text");
				menuPicture.Location = new Point(6 + lblConstantValue.Width, menuPicture.Location.Y);
				stringMenuOption.Visible = forcedType != ConstantValueTypes.String;
				multilineStringMenuOption.Visible = forcedType != ConstantValueTypes.MultilineString;
				singleCharacterMenuOption.Visible = forcedType != ConstantValueTypes.Char;
				booleanMenuOption.Visible = forcedType != ConstantValueTypes.Boolean;
				integerNumberMenuOption.Visible = forcedType != ConstantValueTypes.Integer;
				floatingPointNumberMenuOption.Visible = forcedType != ConstantValueTypes.Double;
			}
		}
	}

	public DotNetExpressionArgument ConstantValue
	{
		set
		{
			if (value.IsBooleanLiteral())
			{
				comboBooleanValues.SelectedIndex = ((!(value.GetString().Trim() == "true")) ? 1 : 0);
				ForcedType = ConstantValueTypes.Boolean;
				return;
			}
			string text = value.GetString().Trim();
			if (value.IsCharLiteral())
			{
				txtConstantValue.Text = text.Substring(1, text.Length - 2);
				ForcedType = ConstantValueTypes.Char;
			}
			else if (value.IsIntegerLiteral())
			{
				txtConstantValue.Text = text;
				ForcedType = ConstantValueTypes.Integer;
			}
			else if (value.IsDoubleLiteral())
			{
				txtConstantValue.Text = text;
				ForcedType = ConstantValueTypes.Double;
			}
			else
			{
				string text2 = (value.IsStringLiteral() ? ExpressionHelper.UnescapeConstantString(text) : text);
				txtConstantValue.Text = text2;
				ForcedType = ((!text2.Contains("\n")) ? ConstantValueTypes.String : ConstantValueTypes.MultilineString);
			}
		}
	}

	private void MenuPicture_Click(object sender, EventArgs e)
	{
		contextMenu.Show(menuPicture, 10, 10);
	}

	private void StringMenuOption_Click(object sender, EventArgs e)
	{
		ForcedType = ConstantValueTypes.String;
		UpdateExpression();
	}

	private void MultilineStringMenuOption_Click(object sender, EventArgs e)
	{
		ForcedType = ConstantValueTypes.MultilineString;
		UpdateExpression();
	}

	private void SingleCharacterMenuOption_Click(object sender, EventArgs e)
	{
		ForcedType = ConstantValueTypes.Char;
		UpdateExpression();
	}

	private void BooleanMenuOption_Click(object sender, EventArgs e)
	{
		ForcedType = ConstantValueTypes.Boolean;
		UpdateExpression();
	}

	private void IntegerNumberMenuOption_Click(object sender, EventArgs e)
	{
		ForcedType = ConstantValueTypes.Integer;
		UpdateExpression();
	}

	private void FloatingPointNumberMenuOption_Click(object sender, EventArgs e)
	{
		ForcedType = ConstantValueTypes.Double;
		UpdateExpression();
	}

	private void DeleteButton_Click(object sender, EventArgs e)
	{
		(base.Parent?.Parent as ExpressionEditorRowControl)?.DeleteRow();
	}

	private string GetForcedTypeAsString()
	{
		return forcedType switch
		{
			ConstantValueTypes.String => "String", 
			ConstantValueTypes.MultilineString => "MultilineString", 
			ConstantValueTypes.Char => "SingleCharacter", 
			ConstantValueTypes.Boolean => "Boolean", 
			ConstantValueTypes.Integer => "IntegerNumber", 
			ConstantValueTypes.Double => "FloatingPointNumber", 
			_ => "", 
		};
	}

	private string GetUpdatedExpression()
	{
		if (forcedType == ConstantValueTypes.String || forcedType == ConstantValueTypes.MultilineString)
		{
			return "\"" + ExpressionHelper.EscapeConstantString(txtConstantValue.Text) + "\"";
		}
		if (forcedType == ConstantValueTypes.Char)
		{
			return "'" + txtConstantValue.Text + "'";
		}
		if (forcedType == ConstantValueTypes.Boolean)
		{
			if (comboBooleanValues.SelectedIndex != 0)
			{
				return "false";
			}
			return "true";
		}
		return txtConstantValue.Text;
	}

	private void UpdateExpression()
	{
		(base.Parent?.Parent as ExpressionEditorRowControl)?.UpdateRow(new DotNetExpressionArgument(GetUpdatedExpression()), redraw: false);
	}

	private void TxtConstantValue_TextChanged(object sender, EventArgs e)
	{
		UpdateExpression();
	}

	private void ComboBooleanValues_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateExpression();
	}

	public AbsArgument GetArgument()
	{
		return new DotNetExpressionArgument(GetUpdatedExpression());
	}

	public void UpdateConstantValues()
	{
		UpdateExpression();
	}

	public ExpressionEditorConstantValueControl()
	{
		InitializeComponent();
		comboBooleanValues.SelectedIndex = 0;
		changeTypeMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorConstantValueControl.changeTypeMenuOption.Text");
		stringMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorConstantValueControl.stringMenuOption.Text");
		multilineStringMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorConstantValueControl.multilineStringMenuOption.Text");
		singleCharacterMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorConstantValueControl.singleCharacterMenuOption.Text");
		booleanMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorConstantValueControl.booleanMenuOption.Text");
		integerNumberMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorConstantValueControl.integerNumberMenuOption.Text");
		floatingPointNumberMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorConstantValueControl.floatingPointNumberMenuOption.Text");
		deleteMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorConstantValueControl.deleteMenuOption.Text");
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
		this.txtConstantValue = new System.Windows.Forms.TextBox();
		this.comboBooleanValues = new System.Windows.Forms.ComboBox();
		this.lblConstantValue = new System.Windows.Forms.Label();
		this.menuPicture = new System.Windows.Forms.PictureBox();
		this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.changeTypeMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		this.stringMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		this.multilineStringMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		this.singleCharacterMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		this.booleanMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		this.integerNumberMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		this.floatingPointNumberMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		this.deleteMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		((System.ComponentModel.ISupportInitialize)this.menuPicture).BeginInit();
		this.contextMenu.SuspendLayout();
		base.SuspendLayout();
		this.txtConstantValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtConstantValue.Location = new System.Drawing.Point(0, 24);
		this.txtConstantValue.Name = "txtConstantValue";
		this.txtConstantValue.Size = new System.Drawing.Size(422, 22);
		this.txtConstantValue.TabIndex = 1;
		this.txtConstantValue.TextChanged += new System.EventHandler(TxtConstantValue_TextChanged);
		this.comboBooleanValues.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboBooleanValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBooleanValues.FormattingEnabled = true;
		this.comboBooleanValues.Items.AddRange(new object[2] { "True", "False" });
		this.comboBooleanValues.Location = new System.Drawing.Point(0, 24);
		this.comboBooleanValues.Name = "comboBooleanValues";
		this.comboBooleanValues.Size = new System.Drawing.Size(422, 24);
		this.comboBooleanValues.TabIndex = 3;
		this.comboBooleanValues.SelectedIndexChanged += new System.EventHandler(ComboBooleanValues_SelectedIndexChanged);
		this.lblConstantValue.AutoSize = true;
		this.lblConstantValue.Location = new System.Drawing.Point(0, 0);
		this.lblConstantValue.Name = "lblConstantValue";
		this.lblConstantValue.Size = new System.Drawing.Size(120, 17);
		this.lblConstantValue.TabIndex = 4;
		this.lblConstantValue.Text = "Constant Boolean";
		this.menuPicture.Image = TCX.CFD.Properties.Resources.Expression_Menu;
		this.menuPicture.Location = new System.Drawing.Point(126, -1);
		this.menuPicture.Name = "menuPicture";
		this.menuPicture.Size = new System.Drawing.Size(18, 18);
		this.menuPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.menuPicture.TabIndex = 12;
		this.menuPicture.TabStop = false;
		this.menuPicture.Click += new System.EventHandler(MenuPicture_Click);
		this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.changeTypeMenuOption, this.deleteMenuOption });
		this.contextMenu.Name = "contextMenu";
		this.contextMenu.Size = new System.Drawing.Size(211, 80);
		this.changeTypeMenuOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.stringMenuOption, this.multilineStringMenuOption, this.singleCharacterMenuOption, this.booleanMenuOption, this.integerNumberMenuOption, this.floatingPointNumberMenuOption });
		this.changeTypeMenuOption.Name = "changeTypeMenuOption";
		this.changeTypeMenuOption.Size = new System.Drawing.Size(210, 24);
		this.changeTypeMenuOption.Text = "Change Type";
		this.stringMenuOption.Name = "stringMenuOption";
		this.stringMenuOption.Size = new System.Drawing.Size(233, 26);
		this.stringMenuOption.Text = "String";
		this.stringMenuOption.Click += new System.EventHandler(StringMenuOption_Click);
		this.multilineStringMenuOption.Name = "multilineStringMenuOption";
		this.multilineStringMenuOption.Size = new System.Drawing.Size(233, 26);
		this.multilineStringMenuOption.Text = "Multiline String";
		this.multilineStringMenuOption.Click += new System.EventHandler(MultilineStringMenuOption_Click);
		this.singleCharacterMenuOption.Name = "singleCharacterMenuOption";
		this.singleCharacterMenuOption.Size = new System.Drawing.Size(233, 26);
		this.singleCharacterMenuOption.Text = "Single Character";
		this.singleCharacterMenuOption.Click += new System.EventHandler(SingleCharacterMenuOption_Click);
		this.booleanMenuOption.Name = "booleanMenuOption";
		this.booleanMenuOption.Size = new System.Drawing.Size(233, 26);
		this.booleanMenuOption.Text = "Boolean";
		this.booleanMenuOption.Click += new System.EventHandler(BooleanMenuOption_Click);
		this.integerNumberMenuOption.Name = "integerNumberMenuOption";
		this.integerNumberMenuOption.Size = new System.Drawing.Size(233, 26);
		this.integerNumberMenuOption.Text = "Integer Number";
		this.integerNumberMenuOption.Click += new System.EventHandler(IntegerNumberMenuOption_Click);
		this.floatingPointNumberMenuOption.Name = "floatingPointNumberMenuOption";
		this.floatingPointNumberMenuOption.Size = new System.Drawing.Size(233, 26);
		this.floatingPointNumberMenuOption.Text = "Floating Point Number";
		this.floatingPointNumberMenuOption.Click += new System.EventHandler(FloatingPointNumberMenuOption_Click);
		this.deleteMenuOption.Image = TCX.CFD.Properties.Resources.Edit_Remove;
		this.deleteMenuOption.Name = "deleteMenuOption";
		this.deleteMenuOption.Size = new System.Drawing.Size(210, 24);
		this.deleteMenuOption.Text = "Delete";
		this.deleteMenuOption.Click += new System.EventHandler(DeleteButton_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.menuPicture);
		base.Controls.Add(this.lblConstantValue);
		base.Controls.Add(this.comboBooleanValues);
		base.Controls.Add(this.txtConstantValue);
		base.Name = "ExpressionEditorConstantValueControl";
		base.Size = new System.Drawing.Size(425, 50);
		((System.ComponentModel.ISupportInitialize)this.menuPicture).EndInit();
		this.contextMenu.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
