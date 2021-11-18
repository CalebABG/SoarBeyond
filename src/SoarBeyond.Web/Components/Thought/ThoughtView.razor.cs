using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Web.Components
{
    public partial class ThoughtView
    {
        [Parameter] public Thought Thought { get; set; }

        [Parameter] public bool ShowActionButtons { get; set; }

        [Parameter] public bool ShowHeader { get; set; } = true;

        [Parameter] public EventCallback<Thought> OnDeleteThought { get; set; }

        public ThoughtView()
        {
            Class = "card shadow-sm";
        }

        protected override string ComponentStyles
            => base.ComponentStyles + $"border-left: 8px solid {Thought?.Color}";

        private async Task HandleDeleteThought()
        {
            await OnDeleteThought.InvokeAsync(Thought);
        }
    }
}