﻿/*
   Copyright 2012 Alexandru Albu - sandu.albu@gmail.com

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using KineSis.Utils;
using KineSis.Profiles;
using System.Windows.Media;

namespace KineSis.UserInterface.Entities.Groups
{
    /// <summary>
    /// group for scrolling/zooming in a page
    /// </summary>
    class Scroll : Group
    {
        String Group.Name
        {
            get
            {
                return "scroll";
            }
        }

        Boolean Group.IsActive
        {
            get
            {
                return true;
            }
        }

        private static List<Group> groups = new List<Group>();

        public static List<Group> Groups
        {
            get
            {
                groups = new List<Group>();
                Group main = UIManager.MainGroup;
                groups.Add(main);
                if (!UIManager.ZoomFit)
                {
                    Group fit = new Generic("fit");
                    groups.Add(fit);
                }
                else
                {
                    Group unfit = new Generic("unfit");
                    groups.Add(unfit);
                }
                Group zoomIn = new Generic("zoom_in");
                groups.Add(zoomIn);
                Group zoomOut = new Generic("zoom_out");
                groups.Add(zoomOut);
                return groups;
            }
        }

        private Group parent;

        private Boolean inSession = false;
        private Boolean leftSelected = false;
        private Boolean rightSelected = false;
        private Boolean upSelected = false;
        private Boolean downSelected = false;

        public Scroll(Group parent)
        {
            this.parent = parent;
        }

        void Group.Draw(Canvas c)
        {
            if (UIManager.ShowSecondaryMenu)
            {
                if (!UIManager.inMenuSession)
                {
                    UIManager.initialX = UIManager.FirstHand.X;
                    UIManager.initialY = UIManager.FirstHand.Y;
                    UIManager.inMenuSession = true;
                }

                System.Windows.Media.Brush primaryColor = ProfileManager.ActiveProfile.PrimaryColor;
                System.Windows.Media.Brush secondaryColor = ProfileManager.ActiveProfile.SecondaryColor;
                System.Windows.Media.Brush fill = ColorUtil.FromHTML("#88FFFFFF");

                double centerX = UIManager.initialX;
                double centerY = UIManager.initialY;

                double leftAreaX = UIManager.initialX - 1.25 * UIManager.SUBMENU_DIAMETER;
                double leftAreaY = UIManager.initialY;

                double rightAreaX = UIManager.initialX + 1.25 * UIManager.SUBMENU_DIAMETER;
                double rightAreaY = UIManager.initialY;

                double upAreaX = UIManager.initialX;
                double upAreaY = UIManager.initialY - 1.25 * UIManager.SUBMENU_DIAMETER;

                double downAreaX = UIManager.initialX;
                double downAreaY = UIManager.initialY + 1.25 * UIManager.SUBMENU_DIAMETER;


                if (UIManager.FirstHand.X > rightAreaX - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.X < rightAreaX + UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y > rightAreaY - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y < rightAreaY + UIManager.SUBMENU_DIAMETER / 2)
                {
                    rightSelected = true;
                    leftSelected = false;
                    upSelected = false;
                    downSelected = false;
                }
                else if (UIManager.FirstHand.X > leftAreaX - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.X < leftAreaX + UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y > leftAreaY - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y < leftAreaY + UIManager.SUBMENU_DIAMETER / 2)
                {
                    leftSelected = true;
                    rightSelected = false;
                    upSelected = false;
                    downSelected = false;
                }
                else if (UIManager.FirstHand.X > upAreaX - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.X < upAreaX + UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y > upAreaY - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y < upAreaY + UIManager.SUBMENU_DIAMETER / 2)
                {
                    if (!UIManager.ZoomFit)
                    {
                        UIManager.ZoomIn();
                    }
                    upSelected = true;
                    leftSelected = false;
                    rightSelected = false;
                    downSelected = false;
                }
                else if (UIManager.FirstHand.X > downAreaX - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.X < downAreaX + UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y > downAreaY - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y < downAreaY + UIManager.SUBMENU_DIAMETER / 2)
                {
                    if (!UIManager.ZoomFit)
                    {
                        UIManager.ZoomOut();
                    }
                    downSelected = true;
                    leftSelected = false;
                    rightSelected = false;
                    upSelected = false;
                }
                else if (UIManager.FirstHand.X > centerX - UIManager.MENU_DIAMETER / 2 && UIManager.FirstHand.X < centerX + UIManager.MENU_DIAMETER / 2 && UIManager.FirstHand.Y > centerY - UIManager.MENU_DIAMETER / 2 && UIManager.FirstHand.Y < centerY + UIManager.MENU_DIAMETER / 2)
                {
                    if (leftSelected && Groups[0].IsActive)
                    {
                        UIManager.SelectedGroup = parent;
                    }
                    else if (rightSelected && Groups[1].IsActive)
                    {
                        UIManager.ZoomFit = !UIManager.ZoomFit;
                    }

                    leftSelected = false;
                    rightSelected = false;
                    upSelected = false;
                    downSelected = false;
                }

                CanvasUtil.DrawEllipse(c, centerX, centerY, UIManager.MENU_DIAMETER, UIManager.MENU_DIAMETER, primaryColor, fill, null);

                if (leftSelected)
                {
                    CanvasUtil.DrawEllipse(c, leftAreaX, leftAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, Groups[0].IsActive ? secondaryColor : Brushes.LightGray, fill, System.Windows.Media.Brushes.White);
                }
                else
                {
                    CanvasUtil.DrawEllipse(c, leftAreaX, leftAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, Groups[0].IsActive ? secondaryColor : Brushes.LightGray, fill, null);
                }

                if (rightSelected)
                {
                    CanvasUtil.DrawEllipse(c, rightAreaX, rightAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, Groups[1].IsActive ? secondaryColor : Brushes.LightGray, fill, Brushes.White);
                }
                else
                {
                    CanvasUtil.DrawEllipse(c, rightAreaX, rightAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, Groups[1].IsActive ? secondaryColor : Brushes.LightGray, fill, null);
                }

                if (upSelected)
                {
                    CanvasUtil.DrawEllipse(c, upAreaX, upAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, !UIManager.ZoomFit ? secondaryColor : Brushes.LightGray, fill, Brushes.White);
                }
                else
                {
                    CanvasUtil.DrawEllipse(c, upAreaX, upAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, !UIManager.ZoomFit ? secondaryColor : Brushes.LightGray, fill, null);
                }

                if (downSelected)
                {
                    CanvasUtil.DrawEllipse(c, downAreaX, downAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, !UIManager.ZoomFit ? secondaryColor : Brushes.LightGray, fill, Brushes.White);
                }
                else
                {
                    CanvasUtil.DrawEllipse(c, downAreaX, downAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, !UIManager.ZoomFit ? secondaryColor : Brushes.LightGray, fill, null);
                }

                System.Windows.Controls.Image image0 = ImageUtil.GetResourceImage("zoom");
                CanvasUtil.DrawImageInCircle(c, image0, UIManager.MENU_DIAMETER, centerX, centerY);

                System.Windows.Controls.Image image1 = ImageUtil.GetResourceImage(parent.Name);
                CanvasUtil.DrawImageInCircle(c, image1, UIManager.SUBMENU_DIAMETER, leftAreaX, leftAreaY);

                System.Windows.Controls.Image image2 = ImageUtil.GetResourceImage(Groups[1].Name);
                CanvasUtil.DrawImageInCircle(c, image2, UIManager.SUBMENU_DIAMETER, rightAreaX, rightAreaY);

                System.Windows.Controls.Image image3 = ImageUtil.GetResourceImage(Groups[2].Name);
                CanvasUtil.DrawImageInCircle(c, image3, UIManager.SUBMENU_DIAMETER, upAreaX, upAreaY);

                System.Windows.Controls.Image image4 = ImageUtil.GetResourceImage(Groups[3].Name);
                CanvasUtil.DrawImageInCircle(c, image4, UIManager.SUBMENU_DIAMETER, downAreaX, downAreaY);
            }
            else if (UIManager.FirstHandNumber != 0 && UIManager.FirstHand.IsSelected)
            {
                if (!inSession)
                {
                    UIManager.initialX = UIManager.FirstHand.X;
                    UIManager.initialY = UIManager.FirstHand.Y;
                    inSession = true;
                }

                System.Windows.Media.Brush primaryColor = ProfileManager.ActiveProfile.PrimaryColor;
                System.Windows.Media.Brush secondaryColor = ProfileManager.ActiveProfile.SecondaryColor;
                System.Windows.Media.Brush fill = ColorUtil.FromHTML("#88FFFFFF");

                double centerX = UIManager.initialX;
                double centerY = UIManager.initialY;

                double leftAreaX = UIManager.initialX - 1.25 * UIManager.SUBMENU_DIAMETER;
                double leftAreaY = UIManager.initialY;

                double rightAreaX = UIManager.initialX + 1.25 * UIManager.SUBMENU_DIAMETER;
                double rightAreaY = UIManager.initialY;

                double upAreaX = UIManager.initialX;
                double upAreaY = UIManager.initialY - 1.25 * UIManager.SUBMENU_DIAMETER;

                double downAreaX = UIManager.initialX;
                double downAreaY = UIManager.initialY + 1.25 * UIManager.SUBMENU_DIAMETER;

                if (UIManager.FirstHand.X > rightAreaX - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.X < rightAreaX + UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y > rightAreaY - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y < rightAreaY + UIManager.SUBMENU_DIAMETER / 2)
                {
                    rightSelected = true;
                    leftSelected = false;
                    upSelected = false;
                    downSelected = false;
                    UIManager.ScrollRight();
                }
                else if (UIManager.FirstHand.X > leftAreaX - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.X < leftAreaX + UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y > leftAreaY - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y < leftAreaY + UIManager.SUBMENU_DIAMETER / 2)
                {
                    leftSelected = true;
                    rightSelected = false;
                    upSelected = false;
                    downSelected = false;
                    UIManager.ScrollLeft();
                }
                else if (UIManager.FirstHand.X > upAreaX - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.X < upAreaX + UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y > upAreaY - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y < upAreaY + UIManager.SUBMENU_DIAMETER / 2)
                {
                    upSelected = true;
                    leftSelected = false;
                    rightSelected = false;
                    downSelected = false;
                    UIManager.ScrollUp();
                }
                else if (UIManager.FirstHand.X > downAreaX - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.X < downAreaX + UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y > downAreaY - UIManager.SUBMENU_DIAMETER / 2 && UIManager.FirstHand.Y < downAreaY + UIManager.SUBMENU_DIAMETER / 2)
                {
                    downSelected = true;
                    leftSelected = false;
                    rightSelected = false;
                    upSelected = false;
                    UIManager.ScrollDown();
                }
                else if (UIManager.FirstHand.X > centerX - UIManager.MENU_DIAMETER / 2 && UIManager.FirstHand.X < centerX + UIManager.MENU_DIAMETER / 2 && UIManager.FirstHand.Y > centerY - UIManager.MENU_DIAMETER / 2 && UIManager.FirstHand.Y < centerY + UIManager.MENU_DIAMETER / 2)
                {
                    leftSelected = false;
                    rightSelected = false;
                    upSelected = false;
                    downSelected = false;
                }

                CanvasUtil.DrawEllipse(c, centerX, centerY, UIManager.MENU_DIAMETER, UIManager.MENU_DIAMETER, primaryColor, fill, null);

                if (leftSelected)
                {
                    CanvasUtil.DrawEllipse(c, leftAreaX, leftAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, secondaryColor, fill, Brushes.White);
                }
                else
                {
                    CanvasUtil.DrawEllipse(c, leftAreaX, leftAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, secondaryColor, fill, null);
                }

                if (rightSelected)
                {
                    CanvasUtil.DrawEllipse(c, rightAreaX, rightAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, secondaryColor, fill, Brushes.White);
                }
                else
                {
                    CanvasUtil.DrawEllipse(c, rightAreaX, rightAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, secondaryColor, fill, null);
                }

                if (upSelected)
                {
                    CanvasUtil.DrawEllipse(c, upAreaX, upAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, secondaryColor, fill, Brushes.White);
                }
                else
                {
                    CanvasUtil.DrawEllipse(c, upAreaX, upAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, secondaryColor, fill, null);
                }

                if (downSelected)
                {
                    CanvasUtil.DrawEllipse(c, downAreaX, downAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, secondaryColor, fill, Brushes.White);
                }
                else
                {
                    CanvasUtil.DrawEllipse(c, downAreaX, downAreaY, UIManager.SUBMENU_DIAMETER, UIManager.SUBMENU_DIAMETER, secondaryColor, fill, null);
                }

                System.Windows.Controls.Image image0 = ImageUtil.GetResourceImage(((Group)this).Name);
                CanvasUtil.DrawImageInCircle(c, image0, UIManager.MENU_DIAMETER, centerX, centerY);

                System.Windows.Controls.Image image1 = ImageUtil.GetResourceImage("left");
                CanvasUtil.DrawImageInCircle(c, image1, UIManager.SUBMENU_DIAMETER, leftAreaX, leftAreaY);

                System.Windows.Controls.Image image2 = ImageUtil.GetResourceImage("right");
                CanvasUtil.DrawImageInCircle(c, image2, UIManager.SUBMENU_DIAMETER, rightAreaX, rightAreaY);

                System.Windows.Controls.Image image3 = ImageUtil.GetResourceImage("up");
                CanvasUtil.DrawImageInCircle(c, image3, UIManager.SUBMENU_DIAMETER, upAreaX, upAreaY);

                System.Windows.Controls.Image image4 = ImageUtil.GetResourceImage("down");
                CanvasUtil.DrawImageInCircle(c, image4, UIManager.SUBMENU_DIAMETER, downAreaX, downAreaY);
            }
            else
            {
                inSession = false;
                leftSelected = false;
                rightSelected = false;
                upSelected = false;
                downSelected = false;
            }
        }
    }
}
