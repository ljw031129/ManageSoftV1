using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Core.Common
{

    public class DynatreeNode
    {
        public DynatreeNode()
        {
            activate = false;
            addClass = null;
            expand = true;
            focus = false;
            icon = null;
            href = null;
            unselectable = false;
            select = false;
            noLink = false;
            isLazy = false;
            hideCheckbox = false;
            isFolder = true;
            children = new List<DynatreeNode>();
        }

        #region property
        /// <summary> 
        /// (required) Displayed name of the node (html is allowed here) 
        /// </summary> 
        public string title { get; set; }

        /// <summary> 
        /// tooltip: null, // Show this popup text. 
        /// </summary> 
        public string tooltip { get; set; }

        /// <summary> 
        /// href: null, // Added to the generated a tag. 
        /// </summary> 
        public string href { get; set; }

        /// <summary> 
        /// icon: null, // Use a custom image (filename relative to tree.options.imagePath). 'null' for default icon, 'false' for no icon. 
        /// </summary> 
        public string icon { get; set; }

        /// <summary> 
        /// addClass: null, // Class name added to the node's span tag.     
        /// </summary> 
        public string addClass { get; set; }

        /// <summary> 
        ///  children: null // Array of child nodes. 
        /// </summary> 
        public List<DynatreeNode> children { get; set; }

        /// <summary> 
        /// unselectable: false, // Prevent selection. 
        /// </summary> 
        public bool unselectable { get; set; }

        /// <summary> 
        /// hideCheckbox: false, // Suppress checkbox display for this node. 
        /// </summary> 
        public bool hideCheckbox { get; set; }

        /// <summary> 
        /// select: false, // Initial selected status. 
        /// </summary> 
        public bool select { get; set; }

        /// <summary> 
        /// May be used with activate(), select(), find(), ... 
        /// </summary> 
        public string key { get; set; }

        /// <summary> 
        /// expand: false, // Initial expanded status. 
        /// </summary> 
        public bool expand { get; set; }

        /// <summary> 
        /// focus: false, // Initial focused status. 
        /// </summary> 
        public bool focus { get; set; }

        /// <summary> 
        /// Use a folder icon. Also the node is expandable but not selectable.false 
        /// </summary> 
        public bool isFolder { get; set; }

        /// <summary> 
        /// isLazy: false,  Call onLazyRead(), when the node is expanded for the first time to allow for delayed 
        /// </summary> 
        public bool isLazy { get; set; }

        /// <summary> 
        /// noLink: false, // Use span instead of a tag for this node 
        /// </summary> 
        public bool noLink { get; set; }

        /// <summary> 
        /// activate: false, // Initial active status. 
        /// </summary> 
        public bool activate { get; set; }
        #endregion
       
    }
}
