using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.Permissions;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Misars.Foundation.App.Blazor.Client.Pages;

public partial class Doctors
{
    private IReadOnlyList<DoctorDto> DoctorList { get; set; }

    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }

    private bool CanCreateDoctor { get; set; }
    private bool CanEditDoctor { get; set; }
    private bool CanDeleteDoctor { get; set; }

    private CreateDoctorDto NewDoctor { get; set; }

    private Guid EditingDoctorId { get; set; }
    private UpdateDoctorDto EditingDoctor { get; set; }

    private Modal CreateDoctorModal { get; set; }
    private Modal EditDoctorModal { get; set; }

    private Validations CreateValidationsRef;
    
    private Validations EditValidationsRef;
    
    public Doctors()
    {
        NewDoctor = new CreateDoctorDto();
        EditingDoctor = new UpdateDoctorDto();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetDoctorsAsync();
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateDoctor = await AuthorizationService
            .IsGrantedAsync(AppPermissions.Doctors.Create);

        CanEditDoctor = await AuthorizationService
            .IsGrantedAsync(AppPermissions.Doctors.Edit);

        CanDeleteDoctor = await AuthorizationService
            .IsGrantedAsync(AppPermissions.Doctors.Delete);
    }

    private async Task GetDoctorsAsync()
    {
        var result = await DoctorAppService.GetListAsync(
            new GetDoctorListDto
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting
            }
        );

        DoctorList = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<DoctorDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page - 1;

        await GetDoctorsAsync();

        await InvokeAsync(StateHasChanged);
    }

    private void OpenCreateDoctorModal()
    {
        CreateValidationsRef.ClearAll();
        
        NewDoctor = new CreateDoctorDto();
        CreateDoctorModal.Show();
    }

    private void CloseCreateDoctorModal()
    {
        CreateDoctorModal.Hide();
    }

    private void OpenEditDoctorModal(DoctorDto doctor)
    {
        EditValidationsRef.ClearAll();
        
        EditingDoctorId = doctor.Id;
        EditingDoctor = ObjectMapper.Map<DoctorDto, UpdateDoctorDto>(doctor);
        EditDoctorModal.Show();
    }

    private async Task DeleteDoctorAsync(DoctorDto doctor)
    {
        var confirmMessage = L["AuthorDeletionConfirmationMessage", doctor.Name];
        if (!await Message.Confirm(confirmMessage))
        {
            return;
        }

        await DoctorAppService.DeleteAsync(doctor.Id);
        await GetDoctorsAsync();
    }

    private void CloseEditDoctorModal()
    {
        EditDoctorModal.Hide();
    }

    private async Task CreateDoctorAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await DoctorAppService.CreateAsync(NewDoctor);
            await GetDoctorsAsync();
            CreateDoctorModal.Hide();
        }
    }

    private async Task UpdateDoctorAsync()
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await DoctorAppService.UpdateAsync(EditingDoctorId, EditingDoctor);
            await GetDoctorsAsync();
            EditDoctorModal.Hide();
        }
    }
}
