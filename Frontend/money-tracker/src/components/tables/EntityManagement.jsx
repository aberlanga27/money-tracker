import './EntityManagement.css';
import { api } from "../../boot/axios";
import { Button, NegativeButton } from "../common/Button";
import { currency, date } from "../../utils/formatters";
import { notifyError } from "../../utils/notify";
import { Pagination } from '../common/Pagination';
import { useCallback, useEffect, useId, useMemo, useState } from "react";
import { IndexedSelect } from '../common/IndexedSelect';
import { AddEditModal } from '../modals/AddEditModal';

export function EntityManagement({
    endpoint,
    indexKey,
    displayName,
    properties = [],
    allowAdd = false,
    allowEdit = false,
    allowDelete = false,
    allowMultipleAdd = false
}) {
    const uniqueId = useId();

    const [search, setSearch] = useState("");
    const [filter, setFilter] = useState({});

    const [records, setRecords] = useState([]);
    const [fallbackRecords, setFallbackRecords] = useState([]);
    const [noRecords, setNoRecords] = useState(0);

    const [showAddEditModal, setShowAddEditModal] = useState(false);
    const [recordToEdit, setRecordToEdit] = useState({});
    const [modalMode, setModalMode] = useState("add");

    const displayProperties = useMemo(() => properties.filter(property => property.name !== indexKey), [properties, indexKey]);
    const selectProperties = useMemo(() => properties.filter(property => property.type === 'select'), [properties]);

    // ...

    const getRecordsPerPage = useCallback(({ page, itemsPerPage }) => {
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
    }, [endpoint]);

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

    const togleFilterPopUp = useCallback(() => {
        const filterPopUp = document.querySelector(`#entity-management-${uniqueId} .filter-pop-up`);
        filterPopUp.classList.toggle('visible');
    }, [uniqueId]);

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

    // ...

    useEffect(() => {
        getRecordsPerPage({ page: 1, itemsPerPage: 5 });
    }, [])

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
                    <Button disabled={!allowAdd} onClick={() => {
                        setRecordToEdit({});
                        setModalMode('add');
                        setShowAddEditModal(true);
                    }}>
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
                                    onClear={() => {
                                        const newFilter = { ...filter };
                                        delete newFilter[property.option.value];
                                        setFilter(newFilter);
                                    }}
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
                                                    <Button onClick={() => {
                                                        setModalMode('edit');
                                                        setRecordToEdit(record);
                                                        setShowAddEditModal(true);
                                                    }}>
                                                        <span className="material-icons" style={{ fontSize: '0.8rem' }}>
                                                            edit
                                                        </span>
                                                    </Button>
                                                )
                                            }
                                            {
                                                allowDelete && (
                                                    <NegativeButton className=''>
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
                record={recordToEdit}
                onOk={() => { setShowAddEditModal(false) }}
                onClose={() => { setShowAddEditModal(false) }}
            />

            <Pagination
                noRecords={noRecords}
                itemsPerPage={5}
                onPrevious={getRecordsPerPage} onNext={getRecordsPerPage}
            />
        </div>
    );
}