window.ShowToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Operation Successful");
    }
    if (type === "error") {
        toastr.error(message, "Operation Failed");
    }
}

window.ShowSwal = (type, message) => {
    if (type === "success") {
        
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: message,
            footer: '<a href="">Getting stuff right!</a>'
        })

    }
    if (type === "error") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: message,
            footer: '<a href="">Screwing up too much!</a>'
        })
    }
}

function ShowDeleteConfirmationModel() {
    $("#deleteConfirmationModal").modal('show');
}

function HideDeleteConfirmationModel() {
    $("#deleteConfirmationModal").modal('hide');
}
