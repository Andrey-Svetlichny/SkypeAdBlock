using System;
using System.Linq;
using SkypeAdBlock.NativeWindowOperations;

namespace SkypeAdBlock
{
    /// <summary>
    /// Удаляет рекламные баннеры из скайпа.
    /// </summary>
    public static class SkypeAdBlock
    {
        public static void RemoveAds(bool showInfo = false)
        {
            RemoveBannerTop(showInfo);
            RemoveBannerLeft(showInfo);
        }

        private static void RemoveBannerLeft(bool showInfo = false)
        {
            // левый нижний баннер
            var bannerLeft = new WindowChain { ClassNamesList = new[] { "tSkMainForm", "TDCEmbedBanner", }.ToList() };
            if (!bannerLeft.Find())
            {
                if (showInfo)
                {
                    Console.WriteLine("BannerLeft not found: {0}", bannerLeft);
                }
            }
            else
            {
                Window.Close(bannerLeft.Handle);
            }


            var conversationsControl = new WindowChain
            {
                ClassNamesList = new[] { "tSkMainForm", "TConversationsControl", }.ToList()
            };
            if (!conversationsControl.Find())
            {
                if (showInfo)
                {
                    Console.WriteLine("ConversationsControl not found: {0}", conversationsControl);
                }
            }
            else
            {
                IntPtr handle = conversationsControl.HandleList[conversationsControl.HandleList.Count - 1];

                int gap = 8;

                Window.Rect relativeRect = Window.GetRelativeRect(handle);
                Window.Point parentWindowSize = Window.GetParentWindowSize(handle);

                int w = relativeRect.Right - relativeRect.Left;
                int h = relativeRect.Bottom - relativeRect.Top;
                h = parentWindowSize.Y - relativeRect.Top - gap;

                Window.Move(handle, relativeRect.Left, relativeRect.Top, w, h);
            }
        }

        private static void RemoveBannerTop(bool showInfo = false)
        {
            // верхний баннер
            var bannerTop1 = new WindowChain
            {
                ClassNamesList = new[] { "tSkMainForm", "TChatBanner", "Shell Embedding", "Shell DocObject View" }.ToList()
            };
            if (!bannerTop1.Find())
            {
                if (showInfo)
                {
                    Console.WriteLine("BannerTop1 not found: {0}", bannerTop1);
                }
            }
            else
            {
                Window.Close(bannerTop1.Handle);
            }

            var bannerTop2 = new WindowChain
            {
                ClassNamesList = new[] { "tSkMainForm", "TChatBanner", "TBrowserControl", "Shell Embedding", "Shell DocObject View" }.ToList()
            };
            if (!bannerTop2.Find())
            {
                if (showInfo)
                {
                    Console.WriteLine("BannerTop1 not found: {0}", bannerTop2);
                }
            }
            else
            {
                Window.Close(bannerTop2.Handle);
            }


            var conversationForm = new WindowChain { ClassNamesList = new[] { "tSkMainForm", "TConversationForm", }.ToList() };
            if (!conversationForm.Find())
            {
                if (showInfo)
                {
                    Console.WriteLine("ConversationForm not found: {0}", conversationForm);
                }
            }
            else
            {
                IntPtr handle = conversationForm.HandleList[conversationForm.HandleList.Count - 1];
                Window.Rect relativeRect = Window.GetRelativeRect(handle);

                int dy = -relativeRect.Top;

                Window.Move(handle, relativeRect.Left, relativeRect.Top + dy, relativeRect.Right - relativeRect.Left,
                    relativeRect.Bottom - relativeRect.Top - dy);
            }
        }
    }
}
