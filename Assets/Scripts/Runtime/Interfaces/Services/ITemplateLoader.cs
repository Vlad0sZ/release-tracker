using System;
using UnityEngine.UIElements;

namespace Runtime.Interfaces.Services
{
    public interface ITemplateLoader
    {
        VisualTreeAsset GetTemplate(string treeName);
    }
}