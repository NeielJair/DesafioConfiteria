function showModal(id, title, btnConfirm) {
	var mdl = $('#' + id);
	mdl.find('.modal-title').text(title);
	mdl.find('.modal-confirm-btn').text(btnConfirm);
	mdl.modal('show');
}

function closeModal(id) {
	var mdl = $('#' + id);
	mdl.modal('hide');
}

function swalWarningConfirm(btn, content) {
	if (btn.dataset.confirmed) {
		// Si la acción ya fue confirmada, proceder
		btn.dataset.confirmed = false;
		return true;
	} else {
		// Pedirle confirmación al usuario
		event.preventDefault();
		swal({
			text: content,
			icon: "warning",
			buttons: ["No", "Sí"],
			dangerMode: true
		})
			.then(function (response) {
				if (response) {
					// Confirmar que el próximo evento de click debe ser ignorado
					//en la parte de JS
					btn.dataset.confirmed = true;
					btn.click();
				}
			}).catch(function (reason) {
				// La acción fue cancelada
			});
	}
}