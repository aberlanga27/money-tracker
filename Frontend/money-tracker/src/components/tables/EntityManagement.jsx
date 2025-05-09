import './EntityManagement.css';
import { AddEditModal } from '../modals/AddEditModal';
import { api } from "../../boot/axios";
import { Button, NegativeButton } from "../common/Button";
import { ConfirmationModal } from '../modals/ConfirmationModal';
import { currency, date } from "../../utils/formatters";
import { IndexedSelect } from '../common/IndexedSelect';
import { notifyError } from "../../utils/notify";
import { Pagination } from '../common/Pagination';
import { useCallback, useEffect, useId, useMemo, useState } from "react";

export function EntityManagement({
    endpoint,
    indexKey,
    displayName,
    properties = [],
    itemsPerPage = 5,
    allowAdd = false,
    allowEdit = false,
    allowDelete = false,
    allowMultipleAdd = false,
    onRecordsModified = () => { },
}) {
    const uniqueId = useId();

    const [search, setSearch] = useState("");
    const [filter, setFilter] = useState({});

    const [records, setRecords] = useState([]);
    const [fallbackRecords, setFallbackRecords] = useState([]);
    const [noRecords, setNoRecords] = useState(0);

    const [selectedRecord, setSelectedRecord] = useState({});

    const [showAddEditModal, setShowAddEditModal] = useState(false);
    const [modalMode, setModalMode] = useState("add");

    const [showConfirmationModal, setShowConfirmationModal] = useState(false);

    const displayProperties = useMemo(() => properties.filter(property => property.name !== indexKey), [properties, indexKey]);
    const selectProperties = useMemo(() => properties.filter(property => property.type === 'select'), [properties]);

    // ...

    const getRecordsPerPage = useCallback(({ page }) => {
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
            .catch(error => notifyError({ message: error }));
    }, [endpoint, itemsPerPage]);

    const searchRecordsOnDatabase = useCallback(() => {
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
    }, [endpoint, search, fallbackRecords]);

    const filterRecords = useCallback(() => {
        if (!Object.keys(filter).length) {
            setRecords(fallbackRecords);
            return;
        }

        api.post(`/${endpoint}/Find`, filter)
            .then(({ data }) => {
                setRecords(data);
            })
            .catch((error) => notifyError({ message: error }))
    }, [endpoint, filter, fallbackRecords]);

    const togleFilterPopUp = useCallback(() => {
        const filterPopUp = document.querySelector(`#entity-management-${uniqueId} .filter-pop-up`);
        filterPopUp.classList.toggle('visible');
    }, [uniqueId]);

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
        getRecordsPerPage({ page: 1 });

        return () => {
            setRecords([]);
            setFallbackRecords([]);
            setNoRecords(0);
            setSearch("");
            setFilter({});
            setShowAddEditModal(false);
            setSelectedRecord({});
            setModalMode("add");
        }
    }, [getRecordsPerPage])

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
        <div id={`entity-management-${uniqueId}`} className='entity-management-table text-sm'>
            <div className="actions-bar flex justify-between items-center">
                <input
                    type="text"
                    placeholder={`Search ${displayName}`}
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                    className="p-2 border border-gray-300 rounded-lg"
                />

                <div className="actions flex items-center gap-1">
                    <Button disabled={!allowAdd} onClick={showAddModal}>
                        <span className="material-icons" style={{ fontSize: '1rem' }}>
                            add
                        </span>
                    </Button>

                    <Button disabled={!allowMultipleAdd}>
                        <span className="material-icons" style={{ fontSize: '1rem' }}>
                            topic
                        </span>
                    </Button>

                    <Button disabled={selectProperties.length == 0} onClick={togleFilterPopUp}>
                        <span className="material-icons" style={{ fontSize: '1rem' }}>
                            filter_list
                        </span>
                    </Button>
                </div>
            </div>

            <div className="filter-pop-up">
                <div className='text-primary font-bold'>Filters</div>
                <div className="selectors">
                    {
                        selectProperties.map((property, index) => (
                            <div key={`${uniqueId}-${index}`} className="filter-selectors">
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

            <table className="w-full table-auto bg-secondary/30 rounded-lg my-2">
                <thead>
                    <tr className="bg-primary text-white">
                        {
                            displayProperties.map((property, index) => (
                                <th key={index} className="p-2">{property.display}</th>
                            ))
                        }
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        records.map((record, index) => (
                            <tr key={index} className="border-b border-gray-300">
                                {
                                    displayProperties.map((property, index) => (
                                        <td key={index} className="p-2">
                                            {
                                                property.format === 'currency' ? currency(record[property.name]) :
                                                property.type === 'number' ? record[property.name] :
                                                property.type === 'date' ? date(record[property.name]) :
                                                property.type === 'select' ? record[property.option.label] :
                                                record[property.name]
                                            }
                                        </td>
                                    ))
                                }
                                {
                                    (allowEdit || allowDelete) && (
                                        <td className="p-2 flex justify-center items-center gap-1">
                                            {
                                                allowEdit && (
                                                    <Button onClick={() => showEditModal(record)}>
                                                        <span className="material-icons" style={{ fontSize: '0.8rem' }}>
                                                            edit
                                                        </span>
                                                    </Button>
                                                )
                                            }
                                            {
                                                allowDelete && (
                                                    <NegativeButton onClick={() => {showDeleteModal(record)}}>
                                                        <span className="material-icons" style={{ fontSize: '0.8rem' }}>
                                                            delete
                                                        </span>
                                                    </NegativeButton>
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

            <AddEditModal endpoint={endpoint} displayName={displayName} indexKey={indexKey}
                properties={displayProperties} modalMode={modalMode}
                show={showAddEditModal}
                record={selectedRecord}
                onOk={onRecordAction}
                onClose={() => { setShowAddEditModal(false) }}
            />

            <ConfirmationModal title='Delete record' message={`Are you sure you want to delete this record?`}
                show={showConfirmationModal}
                onOk = {onConfirmationDeleteRecord}
                onClose={() => { setShowConfirmationModal(false) }}
            />

            <Pagination
                noRecords={noRecords}
                itemsPerPage={itemsPerPage}
                onPrevious={getRecordsPerPage} onNext={getRecordsPerPage}
            />
        </div>
    );
}