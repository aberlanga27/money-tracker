import { AddEditModal } from '../modals/AddEditModal';
import { api } from "../../boot/axios";
import { ConfirmationModal } from '../modals/ConfirmationModal';
import { currency, date } from "../../utils/formatters";
import { IconButton, NegativeIconButton } from "../common/Button";
import { IndexedSelect } from '../common/IndexedSelect';
import { Input } from '../common/Input';
import { LoadingBar } from '../common/LoadingBar';
import { notifyError } from "../../utils/notify";
import { Pagination } from '../common/Pagination';
import { useCallback, useEffect, useId, useMemo, useState } from "react";

/* TODOS
    - Pop up modal of filter, close on click outside
    - Add multiple add modal using numbers, csv or excel
    - Add doc
*/

export function EntityManagement({
    endpoint,
    indexKey,
    displayName,
    properties = [],
    defaultItemsPerPage = 5,
    allowAdd = false,
    allowEdit = false,
    allowDelete = false,
    allowMultipleAdd = false,
    onRecordsModified = () => { },
}) {
    const uniqueId = useId();
    const componentId = `entity-management-${uniqueId}`;

    const [loading, setLoading] = useState(false);
    const [search, setSearch] = useState("");
    const [filter, setFilter] = useState({});

    const [records, setRecords] = useState([]);
    const [fallbackRecords, setFallbackRecords] = useState([]);
    const [noRecords, setNoRecords] = useState(0);

    const [disablePagination, setDisablePagination] = useState(false);

    const [modalMode, setModalMode] = useState("add");
    const [selectedRecord, setSelectedRecord] = useState({});
    const [showAddEditModal, setShowAddEditModal] = useState(false);
    const [showConfirmationModal, setShowConfirmationModal] = useState(false);

    const displayProperties = useMemo(() => properties.filter(property => property.name !== indexKey), [properties, indexKey]);
    const selectProperties = useMemo(() => properties.filter(property => property.type === 'select'), [properties]);

    // ...

    const getRecordsPerPage = useCallback(({ page, itemsPerPage }) => {
        setLoading(true);

        api.get(`${endpoint}?pageSize=${itemsPerPage}&offsetSize=${(page - 1) * itemsPerPage}`)
            .then(({ data }) => {
                if (!data.status) {
                    notifyError({ message: data.message });
                    return;
                }

                setRecords(data.response);
                setFallbackRecords(data.response);
                setNoRecords(data.totalRecords);
            })
            .catch(error => notifyError({ message: error }))
            .finally(() => setLoading(false));
    }, [endpoint]);

    const searchRecordsOnDatabase = useCallback(() => {
        setLoading(true);

        const needle = search?.trim().toLowerCase();
        if (!needle || needle.length < 3) {
            setRecords(fallbackRecords);
            return;
        }

        api.get(`/${endpoint}/Search`, { params: { search: needle } })
            .then(({ data }) => {
                setRecords(data);
            })
            .catch((error) => notifyError({ message: error }))
            .finally(() => setLoading(false));
    }, [endpoint, search, fallbackRecords]);

    const filterRecords = useCallback(() => {
        if (Object.keys(filter).length == 0) {
            setRecords(fallbackRecords);
            setDisablePagination(false);
            return;
        }

        setLoading(true);
        setDisablePagination(true);

        api.post(`/${endpoint}/Find`, filter)
            .then(({ data }) => {
                setRecords(data);
            })
            .catch((error) => notifyError({ message: error }))
            .finally(() => setLoading(false));
    }, [endpoint, filter, fallbackRecords]);

    const togleFilterPopUp = useCallback(() => {
        const filterPopUp = document.querySelector(`#${componentId} .filter-pop-up`);
        filterPopUp.classList.toggle('hidden');
    }, [componentId]);

    const clearFilterProperty = useCallback((property) => {
        const newFilter = { ...filter };
        delete newFilter[property.option.value];
        setFilter(newFilter);
    }, [filter]);

    const showAddModal = useCallback(() => {
        setModalMode('add');
        setSelectedRecord({});
        setShowAddEditModal(true);
    }, []);

    const showEditModal = useCallback((record) => {
        setModalMode('edit');
        setSelectedRecord(record);
        setShowAddEditModal(true);
    }, []);

    const showDeleteModal = useCallback((record) => {
        setSelectedRecord(record);
        setShowConfirmationModal(true);
    }, []);

    const onRecordAdded = useCallback((record) => {
        setRecords((prevRecords) => [record, ...prevRecords]);
        setShowAddEditModal(false);
    }, []);

    const onRecordUpdated = useCallback((record) => {
        setRecords((prevRecords) => {
            const index = prevRecords.findIndex((r) => r[indexKey] === record[indexKey]);
            if (index !== -1) {
                const updatedRecords = [...prevRecords];
                updatedRecords[index] = record;
                return updatedRecords;
            }
            return prevRecords;
        });
        setShowAddEditModal(false);
    }, [indexKey]);

    const onConfirmationDeleteRecord = useCallback(() => {
        api.delete(`/${endpoint}/${selectedRecord[indexKey]}`)
            .then(({ data }) => {
                if (!data.status) {
                    notifyError({ message: data.message });
                    return;
                }

                setRecords((prevRecords) => prevRecords.filter((record) => record[indexKey] !== selectedRecord[indexKey]));
                onRecordsModified(selectedRecord);
            })
            .catch((error) => notifyError({ message: error }))
            .finally(() => {
                setShowConfirmationModal(false);
                setSelectedRecord({});
            });
    }, [endpoint, indexKey, selectedRecord, onRecordsModified]);

    const onRecordAction = useCallback((record) => {
        switch (modalMode) {
            case 'add':
                onRecordAdded(record);
                break;
            case 'edit':
                onRecordUpdated(record);
                break;
            default:
                break;
        }
        onRecordsModified(record);
    }, [modalMode, onRecordAdded, onRecordUpdated, onRecordsModified]);

    // ...

    useEffect(() => {
        getRecordsPerPage({ page: 1, itemsPerPage: defaultItemsPerPage });

        return () => {
            setLoading(false);
            setSearch("");
            setFilter({});
            setRecords([]);
            setFallbackRecords([]);
            setNoRecords(0);
            setDisablePagination(false);
            setModalMode("add");
            setSelectedRecord({});
            setShowAddEditModal(false);
            setShowConfirmationModal(false)
        }
    }, [getRecordsPerPage, defaultItemsPerPage])

    useEffect(() => {
        const timeoutId = setTimeout(() => {
            searchRecordsOnDatabase();
        }, 500);

        return () => clearTimeout(timeoutId);
    }, [search, searchRecordsOnDatabase]);

    useEffect(() => {
        const timeoutId = setTimeout(() => {
            filterRecords();
        }, 500);

        return () => clearTimeout(timeoutId);
    }, [filter, filterRecords]);

    return (
        <div id={componentId} className='entity-management-table text-sm'>
            <div className="actions-bar flex justify-between items-end mb-1">
                <Input
                    type="text"
                    placeholder="Search"
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                >
                    <span className="material-icons pl-2 pr-0 text-gray-500" style={{ fontSize: '1.2rem' }}>
                        search
                    </span>
                </Input>

                <div className="actions flex gap-1">
                    <IconButton icon="add" disabled={!allowAdd} onClick={showAddModal} />

                    {
                        allowMultipleAdd && (
                            <IconButton icon="topic" disabled={!allowMultipleAdd} onClick={showAddModal} />
                        )
                    }

                    <IconButton icon="filter_list" disabled={selectProperties.length == 0} onClick={togleFilterPopUp} />
                </div>
            </div>

            <div className="filter-pop-up hidden bg-white w-[300px] rounded-md p-2 shadow-lg border-1 border-gray-300/70 z-900 absolute right-2">
                <div className='text-primary font-bold mb-1'>Filters</div>
                <div className="selectors flex flex-col gap-1">
                    {
                        selectProperties.map((property, index) => (
                            <div key={`${componentId}-select-${index}`} className="filter-selectors">
                                <IndexedSelect
                                    endpoint={property.option.name} label={property.display}
                                    optionLabel={property.option.label} optionValue={property.option.value}
                                    onChange={({ value }) => setFilter({ ...filter, [property.option.value]: value })}
                                    onClear={() => clearFilterProperty(property)}
                                />
                            </div>
                        ))
                    }
                </div>
            </div>

            <LoadingBar loading={loading} />

            <table className="w-full table-auto bg-secondary/30 rounded-lg mt-0.5 mb-2 overflow-hidden min-h-[13.5rem]">
                <thead className="sticky top-0">
                    <tr className="bg-primary text-white table w-full table-fixed">
                        {
                            displayProperties.map((property, index) => (
                                <th key={`${componentId}-header-${index}`} className="p-2 text-left font-bold">{property.display}</th>
                            ))
                        }
                        <th className="p-2 text-left font-bold">Actions</th>
                    </tr>
                </thead>
                <tbody className="block max-h-[11rem] overflow-y-auto">
                    {
                        records.map((record, index) => (
                            <tr key={`${componentId}-record-${index}`} className="border-b border-gray-300 table w-full table-fixed">
                                {
                                    displayProperties.map((property, index) => (
                                        <td key={`${componentId}-record-prop-${index}`} className="p-2 text-left">
                                            {
                                                property.format === 'currency' ? currency(record[property.name]) :
                                                    property.type === 'number' ? record[property.name] :
                                                        ['date', 'datetime', 'datetime-local'].includes(property.type) ? date(record[property.name]) :
                                                            property.type === 'select' ? record[property.option.label] :
                                                                record[property.name]
                                            }
                                        </td>
                                    ))
                                }
                                {
                                    (allowEdit || allowDelete) && (
                                        <td className="p-2 flex justify-center items-center text-left gap-1">
                                            {
                                                allowEdit && (
                                                    <IconButton icon="edit" fontSize='0.8rem' onClick={() => showEditModal(record)} />
                                                )
                                            }
                                            {
                                                allowDelete && (
                                                    <NegativeIconButton icon="delete" fontSize='0.8rem' onClick={() => { showDeleteModal(record) }} />
                                                )
                                            }
                                        </td>
                                    )
                                }
                            </tr>
                        ))
                    }
                </tbody>
            </table>

            <Pagination
                noRecords={noRecords}
                disabled={disablePagination}
                defaultItemsPerPage={defaultItemsPerPage}
                onPrevious={getRecordsPerPage} onNext={getRecordsPerPage}
            />

            {/* ... */}

            <AddEditModal endpoint={endpoint} displayName={displayName} indexKey={indexKey}
                properties={displayProperties} modalMode={modalMode}
                show={showAddEditModal}
                record={selectedRecord}
                onOk={onRecordAction}
                onClose={() => { setShowAddEditModal(false) }}
            />

            <ConfirmationModal title='Delete record' message={`Are you sure you want to delete this record?`}
                show={showConfirmationModal}
                onOk={onConfirmationDeleteRecord}
                onClose={() => { setShowConfirmationModal(false) }}
            >
                <div className="flex flex-col gap-0 bg-secondary/30 p-2 rounded-lg">
                    {
                        displayProperties.map((property, index) => (
                            <p key={`${componentId}-confirmation-${index}`} className="text-xs">
                                {
                                    <span>
                                        <span className="font-bold">{property.display}: </span>
                                        {
                                            property.format === 'currency' ? currency(selectedRecord[property.name]) :
                                                property.type === 'number' ? selectedRecord[property.name] :
                                                    ['date', 'datetime', 'datetime-local'].includes(property.type) ? date(selectedRecord[property.name]) :
                                                        property.type === 'select' ? selectedRecord[property.option.label] :
                                                            selectedRecord[property.name]
                                        }
                                    </span>
                                }
                            </p>
                        ))
                    }
                </div>
            </ConfirmationModal>
        </div>
    );
}