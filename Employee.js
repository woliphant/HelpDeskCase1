$(function () {
    getAll("");

    $("#main").click(function (e) { // click on any row

        if (!e) e = window.event;
        var empId = e.target.parentNode.id;

        if (empId == "main") {
            empId = e.target.id;    // clicked on row somewhere else
        }

        if (empId != "employee") {
            getById(empId);
            $("#ButtonAction").prop("value", "Update");
            $("#ButtonDelete").prop("value", "Delete");
        }
        else {                  // reset fields
            $("#ButtonDelete").hide();
            $("#ButtonAction").prop("value", "Add");
            $("#HiddenId").val("new");
            $("#TextBoxTitle").val("");
            $("#TextBoxFirstname").val("");
            $("#TextBoxLastname").val("");
            $("#TextBoxPhone").val("");
            $("#TextBoxEmail").val("");
            $("#ButtonUpdate").prop("value", "Add");
            $("#ButtonDelete").hide();
            loadDepartmentDDL(-1);
        }

        //var validator = $("#EmployeeModalForm").validate();
        //validator.resetForm();
    });
})

// Purpose: Builds the initial table for Employe HTML. Populated with an employee list.
function buildTable(data) {
    $("#main").empty();
    var bg = false;
    ajaxCall("Get", "api/employees/", "")
    .done(function (data) {
        employees = data;
        div = $("<div id=\"employee\" data-toggle=\"modal\" data-target=\"#myModal\" class=\"row trWhite\">");
        div.html("<div class=\"col-lg-12\" id=\"id0\">...Click Here to add<\div>");
        div.appendTo($("#main"));
        $.each(data, function (index, emp) {
            var cls = "rowWhite";
            bg ? cls = "rowWhite" : cls = "rowLightGray";
            bg = !bg;
            div = $("<div id=\"" + emp.EmployeeId + "\" data-toggle=\"modal\" data-target=\"#myModal\" class=\"row col-lg-12 " + cls + "\">");
            var empId = emp.EmployeeId;
            div.html(
                "<div class=\"col-xs-4\" id=\"employeetitle" + empId + "\">" + emp.Title + "</div>" +
                "<div class=\"col-xs-4\" id=\"employeefname" + empId + "\">" + emp.Firstname + "</div>" +
                "<div class=\"col-xs-4\" id=\"emplastname" + empId + "\">" + emp.Lastname + "</div>"
                );
            div.appendTo($("#main"));
        });
    });
}

// Purpose: Copies employee information to modal.
// Accepts: Employee Object.
function copyInfoToModal(emp) {
    $("#TextBoxTitle").val(emp.Title);
    $("#TextBoxFirstname").val(emp.Firstname);
    $("#TextBoxLastname").val(emp.Lastname);
    $("#TextBoxPhone").val(emp.Phoneno);
    $("#TextBoxEmail").val(emp.Email);
    $("#HiddenId").val(emp.EmployeeId);
    $("#HiddenEntity").val(emp.Entity64);
    loadDepartmentDDL(emp.DepartmentId);
}

// Purpose: load drop down box of department names
function loadDepartmentDDL(empdep) {
    $.ajax({
        type: "Get",
        url: "api/departments/",
        contentType: "application/json; charset=utf-8"
    })
    .done(function (data) {
        html = "";
        $("#ddlDepts").empty();
        $.each(data, function () {
            html += "<option value=\"" + this["DepartmentId"] + "\">" + this["DepartmentName"] + "</option>";
        });
        $("#ddlDepts").append(html);
        $("#ddlDepts").val(empdep);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("error");
    });
}

// purpose: retrieves all employees from the database and forms a list.
function getAll(msg) {
    $("#LabelStatus").text("Employees loading...");

    ajaxCall("Get", "api/employees", "")
    .done(function (data) {
        buildTable(data);
        if (msg == "")
            $("#LabelStatus").text("Employees Loaded");
        else
            $("#LabelStatus").text(msg + " - " + "Employees Loaded");
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

// Purpose: retrieves an employee by their ID
function getById(empId) {
    ajaxCall("Get", "api/employees/" + empId, "")
    .done(function (data) {
        copyInfoToModal(data);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

// creates a new delete-able object
function _delete() {
    emp = new Object();
    emp.EmployeeId = $("#HiddenId").val();
}

// creates a new employee object to update
function update() {
    emp = new Object();
    emp.Title = $("#TextBoxTitle").val();
    emp.Firstname = $("#TextBoxFirstname").val();
    emp.Lastname = $("#TextBoxLastname").val();
    emp.Phoneno = $("#TextBoxPhone").val();
    emp.Email = $("#TextBoxEmail").val();
    emp.DepartmentId = $("#ddlDepts").val();
    emp.EmployeeId = $("#HiddenId").val();
    emp.Entity64 = $("#HiddenEntity").val();
}

// creates a new employee object to add
function create() {
    emp = new Object();
    emp.Title = $("#TextBoxTitle").val();
    emp.Firstname = $("#TextBoxFirstname").val();
    emp.Lastname = $("#TextBoxLastname").val();
    emp.Phoneno = $("#TextBoxPhone").val();
    emp.Email = $("#TextBoxEmail").val();
    emp.DepartmentId = $("#ddlDepts").val();
}

// Delete button, handles delete method trigger.
$("#ButtonDelete").click(function () {
    var deleteEmp = confirm("really delete this employee?");
    if (deleteEmp) {
    _delete();
    ajaxCall("Delete", "api/employees/", emp)
        .done(function (data) {
            $("myModal").modal("toggle");
            getAll(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $("#myModal").modal("toggle");
            errorRoutine(jqXHR);
        });
    }
    return deleteEmp;
});

// Purpose: Action button, takes care of update and add functions.
$("#ButtonAction").click(function () {
    if ($("#ButtonAction").val() === "Update") {
        $("#ModalStatus").text("Loading...");
        update();
        ajaxCall("Put", "api/employees/", emp)
            .done(function (data) {
                $("#myModal").modal("toggle");
                $("#LabelStatus").text(data);
                buildTable(data);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $("#myModal").modal("toggle");
                errorRoutine(jqXHR);
            });
            return false;   // make sure to return false for click or REST calls get cancelled      
        $("#ModalStatus").text("");
    }
    else {
        create();
        ajaxCall("Post", "api/employees", emp)
        .done(function (data) {
            $("#myModal").modal("toggle");
            $("#LabelStatus").text(emp + " created");
            buildTable(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $("#myModal").modal("toggle");
            errorRoutine(jqXHR);
        });
        return false;
        $("#ModalStatus").text("");
    }
    return false;
});

// Purpose: ajaxCall function, return the promise that '$.ajax' returns
function ajaxCall(type, url, data) {
    return $.ajax({
        type: type,
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: true
    });
}

// Purpose: Main error routine, catches common errors
function errorRoutine(jqXHR) {
    if (jqXHR.responseJSON == null) {
        $("#LabelStatus").text(jqXHR.responseText);
    }
    else {
        $("#LabelStatus").text(jqXHR.responseJSON.Message);
    }
}
