using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Web;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Misars.Foundation.App.SurgeryTimetables;
using Misars.Foundation.App.Permissions;
using Misars.Foundation.App.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;



namespace Misars.Foundation.App.Blazor.Client.Pages
{
    public partial class SurgeryTimetables
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<SurgeryTimetableWithNavigationPropertiesDto> SurgeryTimetableList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateSurgeryTimetable { get; set; }
        private bool CanEditSurgeryTimetable { get; set; }
        private bool CanDeleteSurgeryTimetable { get; set; }
        private SurgeryTimetableCreateDto NewSurgeryTimetable { get; set; }
        private Validations NewSurgeryTimetableValidations { get; set; } = new();
        private SurgeryTimetableUpdateDto EditingSurgeryTimetable { get; set; }
        private Validations EditingSurgeryTimetableValidations { get; set; } = new();
        private string EditingSurgeryTimetableId { get; set; }
        private Modal CreateSurgeryTimetableModal { get; set; } = new();
        private Modal EditSurgeryTimetableModal { get; set; } = new();
        private GetSurgeryTimetablesInput Filter { get; set; }
        private DataGridEntityActionsColumn<SurgeryTimetableWithNavigationPropertiesDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "surgeryTimetable-create-tab";
        protected string SelectedEditTab = "surgeryTimetable-edit-tab";
        private SurgeryTimetableWithNavigationPropertiesDto? SelectedSurgeryTimetable;
        private IReadOnlyList<LookupDto<Guid>> Doctors { get; set; } = new List<LookupDto<Guid>>();
        
        private string SelectedDoctorId { get; set; }
        
        private string SelectedDoctorText { get; set; }

        private Blazorise.Components.Autocomplete<LookupDto<Guid>, string> SelectedDoctorAutoCompleteRef { get; set; } = new();

        private List<LookupDto<Guid>> SelectedDoctors { get; set; } = new List<LookupDto<Guid>>();
        
        
        
        
        
        public SurgeryTimetables()
        {
            NewSurgeryTimetable = new SurgeryTimetableCreateDto();
            EditingSurgeryTimetable = new SurgeryTimetableUpdateDto();
            Filter = new GetSurgeryTimetablesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            SurgeryTimetableList = new List<SurgeryTimetableWithNavigationPropertiesDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetDoctorLookupAsync();


            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
                await SetBreadcrumbItemsAsync();
                await SetToolbarItemsAsync();
                await InvokeAsync(StateHasChanged);
            }
        }  

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["SurgeryTimetables"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            
            
            Toolbar.AddButton(L["NewSurgeryTimetable"], async () =>
            {
                await OpenCreateSurgeryTimetableModalAsync();
            }, IconName.Add, requiredPolicyName: AppPermissions.SurgeryTimetables.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateSurgeryTimetable = await AuthorizationService
                .IsGrantedAsync(AppPermissions.SurgeryTimetables.Create);
            CanEditSurgeryTimetable = await AuthorizationService
                            .IsGrantedAsync(AppPermissions.SurgeryTimetables.Edit);
            CanDeleteSurgeryTimetable = await AuthorizationService
                            .IsGrantedAsync(AppPermissions.SurgeryTimetables.Delete);
                            
                            
        }

        private async Task GetSurgeryTimetablesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await SurgeryTimetablesAppService.GetListAsync(Filter);
            SurgeryTimetableList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetSurgeryTimetablesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<SurgeryTimetableWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetSurgeryTimetablesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateSurgeryTimetableModalAsync()
        {
            SelectedDoctors = new List<LookupDto<Guid>>();
            SelectedDoctorId = string.Empty;
            SelectedDoctorText = string.Empty;

            await SelectedDoctorAutoCompleteRef.Clear();

            NewSurgeryTimetable = new SurgeryTimetableCreateDto{
                
                
            };

            SelectedCreateTab = "surgeryTimetable-create-tab";
            
            
            await NewSurgeryTimetableValidations.ClearAll();
            await CreateSurgeryTimetableModal.Show();
        }

        private async Task CloseCreateSurgeryTimetableModalAsync()
        {
            NewSurgeryTimetable = new SurgeryTimetableCreateDto{
                
                
            };
            await CreateSurgeryTimetableModal.Hide();
        }

        private async Task OpenEditSurgeryTimetableModalAsync(SurgeryTimetableWithNavigationPropertiesDto input)
        {
            SelectedEditTab = "surgeryTimetable-edit-tab";
            
            
            var surgeryTimetable = await SurgeryTimetablesAppService.GetWithNavigationPropertiesAsync(input.SurgeryTimetable.Id);
            
            EditingSurgeryTimetableId = surgeryTimetable.SurgeryTimetable.Id;
            EditingSurgeryTimetable = ObjectMapper.Map<SurgeryTimetableDto, SurgeryTimetableUpdateDto>(surgeryTimetable.SurgeryTimetable);
            SelectedDoctors = surgeryTimetable.Doctors.Select(a => new LookupDto<Guid>{ Id = a.Id, DisplayName = a.Name}).ToList();

            
            await EditingSurgeryTimetableValidations.ClearAll();
            await EditSurgeryTimetableModal.Show();
        }

        private async Task DeleteSurgeryTimetableAsync(SurgeryTimetableWithNavigationPropertiesDto input)
        {
            await SurgeryTimetablesAppService.DeleteAsync(input.SurgeryTimetable.Id);
            await GetSurgeryTimetablesAsync();
        }

        private async Task CreateSurgeryTimetableAsync()
        {
            try
            {
                if (await NewSurgeryTimetableValidations.ValidateAll() == false)
                {
                    return;
                }
                NewSurgeryTimetable.DoctorIds = SelectedDoctors.Select(x => x.Id).ToList();


                await SurgeryTimetablesAppService.CreateAsync(NewSurgeryTimetable);
                await GetSurgeryTimetablesAsync();
                await CloseCreateSurgeryTimetableModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditSurgeryTimetableModalAsync()
        {
            await EditSurgeryTimetableModal.Hide();
        }

        private async Task UpdateSurgeryTimetableAsync()
        {
            try
            {
                if (await EditingSurgeryTimetableValidations.ValidateAll() == false)
                {
                    return;
                }
                EditingSurgeryTimetable.DoctorIds = SelectedDoctors.Select(x => x.Id).ToList();


                await SurgeryTimetablesAppService.UpdateAsync(EditingSurgeryTimetableId, EditingSurgeryTimetable);
                await GetSurgeryTimetablesAsync();
                await EditSurgeryTimetableModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }









        protected virtual async Task OnNameChangedAsync(string? name)
        {
            Filter.Name = name;
            await SearchAsync();
        }
        protected virtual async Task OnBirthDateChangedAsync(string? birthDate)
        {
            Filter.BirthDate = birthDate;
            await SearchAsync();
        }
        protected virtual async Task OnDoctorIdChangedAsync(Guid? doctorId)
        {
            Filter.DoctorId = doctorId;
            await SearchAsync();
        }
        

        private async Task GetDoctorLookupAsync(string? newValue = null)
        {
            Doctors = (await SurgeryTimetablesAppService.GetDoctorLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

        private void AddDoctor()
        {
            if (SelectedDoctorId.IsNullOrEmpty())
            {
                return;
            }
            
            if (SelectedDoctors.Any(p => p.Id.ToString() == SelectedDoctorId))
            {
                UiMessageService.Warn(L["ItemAlreadyAdded"]);
                return;
            }

            SelectedDoctors.Add(new LookupDto<Guid>
            {
                Id = Guid.Parse(SelectedDoctorId),
                DisplayName = SelectedDoctorText
            });
        }







    }
}
