using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Core.Common
{
    public interface ITreeItem
    {
    }
    /// <summary>
    /// Tree Item Leaf.
    /// </summary>
    public class TreeItemLeaf : ITreeItem
    {
        /// <summary>
        /// Gets the Title.
        /// </summary>
        public string title;

        /// <summary>
        /// Gets the Tooltip.
        /// </summary>
        public string tooltip;

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string key;

        /// <summary>
        /// Gets the Data.
        /// </summary>
        public string addClass;

        /// <summary>
        /// Gets the Children.
        /// </summary>
        public IList<ITreeItem> children;

        /// <summary>
        /// Gets the rel attr.
        /// </summary>
        public string rel;

        /// <summary>
        /// Gets the State.
        /// </summary>
        public bool isFolder;

        /// <summary>
        /// Gets the State.
        /// </summary>
        public bool isLazy;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeItemLeaf"/> class.
        /// </summary>
        public TreeItemLeaf()
        {
            children = new List<ITreeItem>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeItemLeaf"/> class.
        /// </summary>
        /// <param name="type">The type of node.</param>
        /// <param name="id">The Id of the node.</param>
        /// <param name="title">The Title of the node.</param>
        /// <param name="tooltip">The Tooltip of the node.</param>
        public TreeItemLeaf(String type, Guid id, String title, String tooltip)
        {
            key = id.ToString();
            this.title = title;
            isFolder = false;
            isLazy = false;
            this.tooltip = tooltip;
            children = new List<ITreeItem>();
        }

    }

    /// <summary>
    /// Tree Item.
    /// </summary>
    public class TreeItem : TreeItemLeaf
    {
        /// <summary>
        /// Gets the State.
        /// </summary>
        public new bool isFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeItem"/> class.
        /// </summary>
        public TreeItem()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeItem"/> class.
        /// </summary>
        /// <param name="type">The type of node.</param>
        /// <param name="id">The Id of the node.</param>
        /// <param name="title">The Title of the node.</param>
        /// <param name="tooltip">The tooltip of the node.</param>
        public TreeItem(String type, Guid id, String title, String tooltip)
            : base(type, id, title, tooltip)
        {
            isFolder = true;
            isLazy = true;
        }

    }
}
