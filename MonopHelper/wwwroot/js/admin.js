function ChangeTenant(id, curTenant, name){
    $.ajax({
        type: 'GET',
        url: '/Admin/GetTenants?currentTenant=' + curTenant,
        success: function (tenants){
            Swal.fire({
                title: "Change " + name + "'s tenant?",
                icon: "info",
                input: "select",
                inputOptions: tenants,
                showDenyButton: true,
                confirmButtonText: "Set Tenant",
                denyButtonText: "Cancel"
            }).then((result) => {
                if(result.isConfirmed){
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/ChangeTenant?userId=' + id + '&tenantId=' + result.value,
                        success: function (success){
                            if(success === true){
                                Swal.fire(name + "'s tenant has been set!", "", "success").then((r) => {
                                    location.assign("/Admin");
                                });
                            }
                        }
                    })
                }
            })
        }
    })
}

function RemoveTenant(id, name, isDeleted){
    let title = "Delete " + name;
    let success = "Deleted";
    if(isDeleted === "False"){
        title = "Restore " + name;
        success = "Restored"
    }
    
    Swal.fire({
        title: title + " Tenant?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: '/Admin/SetTenantIsDeleted?id=' + id + '&isDeleted=' + isDeleted,
                success: function (res){
                    Swal.fire("Tenant " + success, "", "success").then((r) => {
                        location.reload();
                    });
                }
            })

        }
    })
}

function ResetPassword(id, name){
    Swal.fire({
        title: "Reset " + name + "'s Password?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: '/Admin/ResetPassword?id=' + id,
                success: function (res){
                    Swal.fire({
                        title: "Password Reset!",
                        html: '<p>Please prompt the user to sign with the following password and then change their password:</p><p class="text-danger"><b>' + res + '</b></p>',
                        icon: "info",
                        showCancelButton: false,
                        confirmButtonText: "OK"
                    }).then((result) => {
                        location.reload();
                    })
                }
            })

        }
    })
}