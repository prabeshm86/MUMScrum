var PageObjects = {
    ReleaseHidden: document.getElementById('hdnReleaseId'),
    SprintHidden: document.getElementById('hdnSprintId'),
    ProductListValue: $('#ProductDropList').val(),
    Role: $('#hdnRole').val()
};

$(document).ready(function () {
    //console.log('document ready value of Proudct : ' + $('#ProductDropList').val());
    // console.log($('value of release ' + "#hdnReleaseId").val());
    if ($("#ProductDropList").val() != 0)
        GetAllRelease();
    if (PageObjects.ReleaseHidden.value.length != 0)
        GetSprintByRelease(PageObjects.ReleaseHidden.value);
});

function GetAllRelease(ProductId) {

    if (ProductId === undefined)
        ProductId = $('#ProductDropList').val();

    var ProductApiURL = "/api/UserStory?ProductId=" + $('#ProductDropList').val();
    $('#ddlRelease').find('option').remove();
    var noneOption = '<option value="">- None - </option>';
    $("#ddlRelease").append(noneOption);
    $.ajax(
        {
            type: "Get",
            url: ProductApiURL,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
                $('#AjaxImageProduct').show();
            },
            error: function () {
                $('#AjaxImageProduct').hide();
                $('#ddlRelease').find('option').remove();
                $('#ddlRelease').prop('disabled', true);
                $('#ddlSprint').find('option').remove();
                $('#ddlSprint').prop('disabled', true);
                // console.log("Please select valid product backlog!");
            },
            success: function (data) {
                $('#AjaxImageProduct').hide();

                if (data.length > 0) {
                    // first remove the current options if any

                    //console.log(data);

                    $('#ddlRelease').prop('disabled', false);
                    $(data).each(function (i) {
                        var optionhtml = '<option value="' +
                            data[i].id + '">' + data[i].releaseName + '</option>';
                        $("#ddlRelease").append(optionhtml);
                    });
                    //  console.log('value of release Hidden Field : ' + PageObjects.ReleaseHidden.value);
                    if (PageObjects.ReleaseHidden.value != "")
                        $('#ddlRelease').val(PageObjects.ReleaseHidden.value);
                }
                else {
                    $('#ddlRelease').prop('disabled', true);
                }
            }
        }
    );
}

function GetSprintByRelease(ReleaseId) {
    // console.log(PageObjects.ReleaseHidden.value.length)
    //  if (PageObjects.ReleaseHidden.value.length > 0)
    // if ($('#ddlRelease').val() != "") {
    if (ReleaseId === undefined)
        ReleaseId = $('#ddlRelease').val();

    
    // if ($('#ddlRelease').val() != null)
    // $('#ddlSprint').prop('disabled', false);
    $('#ddlSprint').find('option').remove();
    var noneOption = '<option value="">- None - </option>';

    if (ReleaseId == "" && PageObjects.Role == "ProductOwner") {
        PageObjects.SprintHidden.value = null;
        $('#ddlSprint').val("");
        return;
    }
    var ReleaseApiURL = "/api/UserStory?ReleaseId=" + ReleaseId;
    $.ajax(
        {
            type: "Get",
            url: ReleaseApiURL,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            error: function () {
                //console.log("Please select valid Release Backlog!");
                $('#AjaxImageRelease').hide();
            },
            beforeSend: function () {
                $('#AjaxImageRelease').show();
            },
            success: function (data) {
                $('#AjaxImageRelease').hide();
                if (data.length > 0) {
                    // first remove the current options if any
                    // console.log(PageObjects.Role);
                    $("#ddlSprint").append(noneOption);
                    $(data).each(function (i) {

                        // console.log(this.Id + this.ReleaseName);
                        // console.log(data);
                        var optionhtml = '<option value="' +
                            data[i].id + '">' + data[i].sprintName + '</option>';
                        $("#ddlSprint").append(optionhtml);

                    });
                    //   console.log("Sprint Hidden value is : " + PageObjects.SprintHidden.value);
                    if (PageObjects.SprintHidden.value != "")
                        $('#ddlSprint').val(PageObjects.SprintHidden.value);

                    if (PageObjects.Role == "ProductOwner") {
                        $('#ddlSprint').prop('disabled', true);
                        if (PageObjects.SprintHidden.value != $('#ddlSprint').val()) {
                            PageObjects.SprintHidden.value = null;
                        }
                    }
                    else
                        $('#ddlSprint').prop('disabled', false);
                }
                else {
                    //$('#ddlSprint').find('option').remove();
                    $('#ddlSprint').prop('disabled', true);
                }
            }

        }
    );
    // }
}