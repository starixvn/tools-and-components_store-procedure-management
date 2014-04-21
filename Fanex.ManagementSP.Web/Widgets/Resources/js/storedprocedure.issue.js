var actionSource = ["A", "D", "E", "Nested"];
var statusSource = ["In Progress", "Done", "UAT", "UAT [_xtest]", "UAT [_xpre]", "Not Required", "Released", "Pending"];

$(document).ready(function () {
    var arrServer = new Array();
    var arrDatabase = new Array();
    var arrServerDatabase = new Array();
    var arrDeveloper = new Array();
    var arrSPIssue = new Array();
    var selectedServerName = "";
    var currentUser = new Object();
    var selectLimit = 20;
    var isAddNew = true;
    var isSetDataAtRowProp = false;

    //    var apiUrl = 'http://10.21.10.19:9911/api/';
    var apiUrl = 'http://localhost:62215/api/';

    // Parsing parameters
    (window.onpopstate = function () {
        var match,
	        pl = /\+/g,  // Regex for replacing addition symbol with a space
	        search = /([^&=]+)=?([^&]*)/g,
	        decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
	        query = window.location.search.substring(1);

        httpQueries = {};
        while (match = search.exec(query))
            httpQueries[decode(match[1])] = decode(match[2]);
    })();

    $.ajax({
        url: apiUrl + 'server',
        async: false,
        success: function (response) {
            $.each(response, function (i, data) {
                arrServer[i] = data;
            });
        }
    });

    $.ajax({
        url: apiUrl + 'database',
        async: false,
        success: function (response) {
            $.each(response, function (i, data) {
                arrDatabase[i] = data;
            });
        }
    });

    $.ajax({
        url: apiUrl + 'user',
        async: false,
        success: function (response) {
            $.each(response, function (i, data) {
                arrDeveloper[i] = data;
            });
        }
    });

    $.ajax({
        url: apiUrl + 'StoreProcedure?id=' + httpQueries.issueId + '&includeChild=true',
        async: false,
        success: function (response) {
            $.each(response, function (index, data) {
                if (data.Id != 0) {
                    arrSPIssue.push(data);
                }
            });
            if (arrSPIssue.length <= 0) {
                arrSPIssue = response[0];
            }
        }
    });

    currentUser = getAuthor(arrDeveloper, httpQueries.author);

    var hiddenColumnRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        td.hidden = true;
        td.innerText = value;
        return td;
    };

    var columnSetting = [
                {
                    type: "autocomplete",
                    data: "StoreProcedure.Server.Name",
                    source: convertToSourceName(arrServer),
                    strict: true
                },
                {
                    type: "autocomplete",
                    data: "StoreProcedure.Database.Name",
                    source: function (query, process) {
                        // Filter database by serverId
                        var instance = $('#storeprocedures').handsontable('getInstance');
                        var pos = instance.getSelected();
                        if (pos != undefined) {
                            selectedServerName = instance.getDataAtCell(pos[0], 0);

                            arrServerDatabase = getDatabasesByServer(arrDatabase, selectedServerName);
                            filterSelect(arrServerDatabase, query, process);
                        }
                        else {
                            var arr = new Array();
                            arr.push(query);
                            process(arr);
                        }
                    },
                    strict: true
                },
                {
                    type: "autocomplete",
                    data: "StoreProcedure.Name",
                    source: function (query, process) {
                        var instance = $('#storeprocedures').handsontable('getInstance');
                        var pos = instance.getSelected();
                        if (pos != undefined) {
                            var rowIndex = pos[0];
                            var rowData = convertToSPIssueOnlyServerAndDatabase(instance, rowIndex);

                            if (rowData != null) {
                                var param = [
                            'serverId=' + rowData.StoreProcedure.Server.Id,
                            'databaseId=' + rowData.StoreProcedure.Database.Id
                        ];

                                $.ajax({
                                    url: apiUrl + 'StoreProcedure?' + param.join('&'),
                                    async: false,
                                    success: function (response) {
                                        var array = new Array();
                                        $.each(response, function (i, data) {
                                            array.push(data.Name);
                                        });

                                        filterSelect(array, query, process);
                                    }
                                });
                            }
                        }
                    },
                    strict: false
                },
                {
                    type: "checkbox",
                    data: "IsFunction"
                },
                {
                    type: "dropdown",
                    data: "ActionName",
                    source: actionSource
                },
                {
                    type: "autocomplete",
                    data: "Developer.Username",
                    source: function (query, process) {
                        var array = new Array();
                        $.each(arrDeveloper, function (index, data) {
                            array[index] = data.Username;
                        });
                        filterSelect(array, query, process);
                    },
                    strict: true
                },
                {
                    type: "dropdown",
                    data: "StatusName",
                    source: statusSource
                },
                { data: "Description" },
                { data: "Modifier.Username", readOnly: true}];

    $("#storeprocedures").handsontable({
        data: arrSPIssue,
        startRows: 0,
        startCols: 9,
        fixedColumnsLeft: 3,
        rowHeaders: true,
        columnSorting: true,
        colWidths: [140, 140, 310, 60, 70, 120, 100, 100, 100],
        colHeaders: function (col) {
            switch (col) {
                case 0:
                    return "<b>Server</b>";
                case 1:
                    return "<b>Database</b>";
                case 2:
                    return "<b>SP/Function</b>";
                case 3:
                    return "<b>Function</b>";
                case 4:
                    return "<b>Action</b>";
                case 5:
                    return "<b>Developer</b>";
                case 6:
                    return "<b>Status</b>";
                case 7:
                    return "<b>Description</b>";
                case 8:
                    return "<b>Modified By</b>";
                default:
                    return "";
            }
        },
        columns: columnSetting,
        minSpareRows: 1,
        fillHandle: true,
        contextMenu: {
            items: {
                "row_above": {
                    disabled: function () {
                        //if first row, disable this option
                        return ($("#storeprocedures").handsontable('getSelected')[0] === 0);
                    }
                },
                "row_below": {},
                "hsep1": "---------",
                "remove_row": {},
                "hsep2": "---------",
                "undo": {},
                "redo": {}
            }
        },
        onChange: function (e, changes, source) {
            if (e != null) {
                if (e.length == 1 && e[0][2] == e[0][3]) {
                    return;
                }

                var instance = $('#storeprocedures').handsontable('getInstance');
                var pos = instance.getSelected();
                if (pos != undefined) {
                    if (e[0][1] == "StoreProcedure.Server.Name") {
                        selectedServerName = e[0][3];

                        arrServerDatabase = getDatabasesByServer(arrDatabase, selectedServerName);

                        instance.setDataAtCell(pos[0], 1, arrServerDatabase[0], arrServerDatabase);
                    }

                    if (e[0][1] != "Modifier.Username") {
                        for (var i = pos[0]; i <= pos[2]; i++) {
                            instance.setDataAtCell(i, 8, currentUser.Username);
                        }
                    }
                }
            }
        },
        afterChange: function (source, change) {
            if (change === 'loadData' || source[0][1] === 'Modifier.Username' || source[0][1] === 'Id') {
                return;
            }

            if (source.length == 1 && source[0][2] == source[0][3]) {
                return;
            }

            var arrRowIndex = new Array();
            for (var i = 0; i < source.length; i++) {
                var row = _.find(arrRowIndex, function (value) {
                    return value == source[i][0];
                });

                if (row == undefined) {
                    arrRowIndex.push(source[i][0]);
                }
            }
            var instance = $('#storeprocedures').handsontable('getInstance');

            for (var i = 0; i < arrRowIndex.length; i++) {
                saveSPIssue(instance, arrRowIndex[i]);
            }
        },
        beforeRemoveRow: function (index, amount) {
            var instance = $('#storeprocedures').handsontable('getInstance');
            deleteSPIssue(instance, index, amount);
        },
        beforeKeyDown: function (event) {
            var instance = $('#storeprocedures').handsontable('getInstance');
            var pos = instance.getSelected();
            if (pos != undefined) {
                if (pos[1] == 0 && pos[3] == 8) {
                    // keyCode : 46 Delete
                    if (event.keyCode === 46) {
                        // deleteSPIssue(instance, pos[0], pos[2] - pos[0] + 1);
                        event.stopImmediatePropagation();
                        instance.alter('remove_row', pos[0], pos[2] - pos[0] + 1);
                        event.preventDefault();
                    }
                }
            }
        }
    });

    function convertToSPIssue(instance, row) {
        if (!isEmptyRow(instance, row)) {
            var rowData = instance.getDataAtRow(row);
            var serverId = _.find(arrServer, function (item) {
                return item.Name === rowData.StoreProcedure.Server.Name;
            }).Id;

            var databaseId = (_.find(arrDatabase, function (item) {
                return item.Name === rowData.StoreProcedure.Database.Name && item.Server.Id === serverId;
            }) || { Id: 0 }).Id;

            var developerId = _.find(arrDeveloper, function (item) {
                return item.Username === rowData.Developer.Username;
            }).Id;

            rowData.StoreProcedure.Server.Id = serverId;
            rowData.StoreProcedure.Database.Id = databaseId;
            rowData.Developer.Id = developerId;
            rowData.Status = convertToStatusId(rowData.StatusName);

            return rowData;
        }

        return null;
    }

    function convertToSPIssueOnlyServerAndDatabase(instance, row) {
        if (!isEmptyServerAndDatabase(instance, row)) {
            var rowData = instance.getDataAtRow(row);
            var serverId = _.find(arrServer, function (item) {
                return item.Name === rowData.StoreProcedure.Server.Name;
            }).Id;

            var databaseId = (_.find(arrDatabase, function (item) {
                return item.Name === rowData.StoreProcedure.Database.Name && item.Server.Id === serverId;
            }) || { Id: 0 }).Id;

            rowData.StoreProcedure.Server.Id = serverId;
            rowData.StoreProcedure.Database.Id = databaseId;

            return rowData;
        }

        return null;
    }

    function saveSPIssue(instance, row) {
        var rowData = convertToSPIssue(instance, row);

        if (rowData != null && isValidSPIssue(rowData)) {

            if (rowData.Issue.Id == null) {
                rowData.Issue.Id = httpQueries.issueId;
            }

            var param = [
			    'id=' + (rowData.Id || 0),
			    'serverId=' + (rowData.StoreProcedure.Server.Id || 0),
			    'databaseId=' + (rowData.StoreProcedure.Database.Id || 0),
			    'name=' + $.trim(rowData.StoreProcedure.Name || ""),
			    'isFunction=' + (rowData.IsFunction || false),
			    'actionName=' + $.trim(rowData.ActionName || ""),
			    'modifiedById=' + currentUser.Id,
			    'status=' + rowData.Status,
			    'description=' + $.trim(rowData.Description || ""),
			    'issueId=' + rowData.Issue.Id,
			    'developerId=' + (rowData.Developer.Id || 0),
                'storedProcedureId=' + (rowData.StoreProcedure.Id || 0),
                'isAddNew=' + isAddNew];

            window.console.log(param);

            $.ajax({
                url: apiUrl + 'storeprocedure?' + param.join('&'),
                type: 'POST',
                success: function (response) {
                    rowData.Id = response;
                    instance.setDataAtRowProp(row, 'Id', response);
                    window.console.log(response);
                }
            });
        }
    };

    function deleteSPIssue(instance, index, amount) {
        for (var i = index; i < index + amount; i++) {
            var spIssue = convertToSPIssue(instance, i);

            $.ajax({
                url: apiUrl + 'storeprocedure?id=' + spIssue.Id + '&modifiedById=' + currentUser.Id,
                type: 'DELETE',
                success: function (response) {
                    window.console.log(response);
                }
            });
        }
    };

    function convertToSourceName(values) {
        var array = new Array();
        $.each(values, function (index, data) {
            array[index] = data.Name;
        });

        return array;
    };

    function getDatabasesByServer(value, serverName) {
        var array = new Array();
        $.each(value, function (index, data) {
            if (serverName === data.Server.Name) {
                array.push(data.Name);
            }
        });

        return array;
    }

    function filterSelect(array, q, p) {
        var regex = new RegExp(q, "i");
        var found = _.filter(array, function (e) {

            return regex.test(e);
        });

        var limit = found.slice(0, selectLimit - 1);
        if (found.length > selectLimit) {
            limit.push("...");
        };

        p(limit);
    };

    function getAuthor(array, author) {
        return (_.find(array, function (dev) {
            return dev.Username === atob(author);
        }) || { Username: "" });
    }

    function isEmptyRow(instance, row) {
        var rowData = instance.getDataAtRow(row);
        if (rowData.StoreProcedure.Server.Name != null
            && rowData.StoreProcedure.Database.Name != null
            && rowData.StoreProcedure.Name != null
            && rowData.ActionName != null
            && rowData.Developer.Username != null
            && rowData.StatusName != null) {
            return false;
        }

        return true;
    }

    function isEmptyServerAndDatabase(instance, row) {
        var rowData = instance.getDataAtRow(row);
        if (rowData.StoreProcedure.Server.Name != null
            && rowData.StoreProcedure.Database.Name != null) {
            return false;
        }

        return true;
    }

    function isValidSPIssue(data) {
        if (data.StoreProcedure.Server.Id == 0
            || data.StoreProcedure.Database.Id == 0
            || data.StoreProcedure.Name == ""
            || data.Developer.Id == 0
            || data.Status.Id == -1
            || data.ActionName == "") {

            return false;
        }

        return true;
    }

    function convertToStatusId(name) {
        var statusId = 0;
        name = name.toLowerCase();
        switch (name) {
            case "in progress":
                statusId = 1;
                break;
            case "done":
                statusId = 2;
                break;
            case "uat [_xtest]":
                statusId = 3;
                break;
            case "uat [_xpre]":
                statusId = 4;
                break;
            case "not required":
                statusId = 5;
                break;
            case "released":
                statusId = 6;
                break;
            case "pending":
                statusId = 7;
                break;
            case "uat":
                statusId = 8;
                break;
            default:
                statusId = -1;
                break;
        }

        return statusId;
    }
});