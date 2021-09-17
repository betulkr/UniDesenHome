$(document).ready(function () {
	dCustomAutoComplete("/graduation/searchdecision", "Decision");
	AssignUpdateInfoPartialEvents();

	if ($("#updategraduationdiplomainfo #DecisionName").val().length > 0) {
		var value = $("#updategraduationdiplomainfo #DecisionName").val();
		console.log(value);
		$("#updategraduationdiplomainfo #Decision").val(value);
	}


});



function dCustomAutoCompleted(uivalue) {
	$("#updategraduationdiplomainfo #DecisionNo").val(uivalue);
}

function dCustomAutoComplete(actionUrl, controlId) {
	$("#updategraduationdiplomainfo #" + controlId).autocomplete({
		source: actionUrl,
		autoFill: true,
		selectFirst: true,
		cacheLength: 0,
		focus: function (event, ui) {
			$(this).val(ui.item.label);
			return false;
		},
		select: function (event, ui) {
			//$("#" + hiddenId).val(ui.item.value);
			//$("#Spec1").val(ui.item.spec1);
			//$("#Spec2").val(ui.item.spec2);
			//$("#Spec3").val(ui.item.spec3);
			$(this).val(ui.item.label);
			dCustomAutoCompleted(ui.item.value, ui.item.label, ui.item.spec1, ui.item.spec2, ui.item.spec3, ui.item.spec4);
			return false;
		}
	});
}

function AssignUpdateInfoPartialEvents() {
	var graduationDisableStatus = $("#GraduationDisableStatus").val();
	var diplomaDisableStatus = $("#DiplomaDisableStatus").val();

	$(".diploma-related").attr(diplomaDisableStatus, true);
	$(".graduation-related").attr(graduationDisableStatus, true);

}