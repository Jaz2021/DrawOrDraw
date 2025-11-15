using Godot;
using System;

public partial class Healthbar : Control
{
	ProgressBar healthBar; 
	public override void _Ready()
	{
		healthBar = GetNode<ProgressBar>("ProgressBar");
		healthBar.MaxValue = 100f;
	}

	public void setMaxValue(double value)
	{
		healthBar.MaxValue = value;
	}

	public void setHealthValue(double value)
	{
		healthBar.Value = value;
	}

	public double getHealthValue()
	{
		return healthBar.Value;
	}
}
