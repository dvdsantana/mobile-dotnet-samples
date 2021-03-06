﻿
using System;
//using AdvancedMap.iOS;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class MapListCell : UITableViewCell
	{
		public int Index { get; set; }

		UILabel title, description;

		public MapListCell()
		{
			AccessibilityIdentifier = "MapListCell";

			title = new UILabel();
			title.Font = UIFont.FromName("Helvetica-Bold", 12);

			description = new UILabel();
			description.Font = UIFont.FromName("Helvetica Neue", 11);
			description.Lines = 0;
			description.LineBreakMode = UILineBreakMode.WordWrap;
			description.TextAlignment = UITextAlignment.Justified;

			AddSubviews(title, description);

			SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		public void Update(MapListRowSource item, int position)
		{
			title.Text = (position + 1) + ". " + item.Title;
			description.Text = item.Description;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat smallPadding = 5;
			nfloat padding = 10;

			nfloat x = padding;
			nfloat y = smallPadding;
			nfloat w = Frame.Width - 2 * padding;
			nfloat h = Frame.Height / 5;

			title.Frame = new CGRect(x, y, w, h);

			y += h;
			h = Frame.Height - (h + smallPadding);

			description.Frame = new CGRect(x, y, w, h);
		}
	}
}

