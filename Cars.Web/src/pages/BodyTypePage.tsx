import { useEffect, useState } from 'react';
import Table from '../components/Table/Table';
import dayjs from 'dayjs';
import { BaseGetResponse } from '../types/BaseRequest';
import Modal from '../components/Modal/Modal';
import Input from '../components/Input/Input';
import { DictionaryItem } from '../types/Dictionary';
import BodyTypeService from '../services/BodyTypeService';
import Notification from '../components/Notification/Notification';

/** Страница справочника типов кузова */
const BodyTypePage = () => {
    const [entities, setEntities] = useState<BaseGetResponse<DictionaryItem>>();
    const [pageNumber, setPageNumber] = useState(1);
    const [editModel, setEditModel] = useState<Partial<DictionaryItem>>();
    const [errorState, setErrorState] = useState<string>();
    const [successMessage, setSuccessMessage] = useState<string | null>(null);

    const fetchData = () => {
        BodyTypeService.GetListAsync({
            pageNumber,
            pageSize: 10,
        })
            .then(response => setEntities({
                ...response,
                entities: response.entities.map(x => ({ ...x, createdOn: dayjs(x.createdOn).format('DD.MM.YYYY HH:mm') }))
            }))
            .catch((e) => console.warn('Ошибка при получении данных', e));
    }

    useEffect(() => {
        fetchData();
    }, [pageNumber]);

    const onEdit = async (index: number) => {
        setEditModel(entities?.entities[index]);
    };

    const onDelete = async (index: number) => {
        const item = entities?.entities[index];

        if (!item?.id) {
            return;
        }

        try {
            await BodyTypeService.DeleteAsync(item.id);
            setSuccessMessage('Успешное удаление');
            fetchData();
        }
        catch (error: any) {
            const text = error?.response?.data?.title ?? "Ошибка при удалении данных";
            setErrorState(text);
        }
    }

    const onSave = async () => {
        if (!editModel) {
            return;
        }

        if (!editModel.name) {
            setErrorState("Необходимо заполнить наименование");
            return;
        }

        try {
            if (editModel.id) {
                await BodyTypeService.UpdateAsync(editModel.id, editModel as DictionaryItem);
            }
            else {
                await BodyTypeService.CreateAsync(editModel as DictionaryItem);
            }

            setSuccessMessage('Успешное сохранение');
            setEditModel(undefined);
            fetchData();
        }
        catch (error: any) {
            const text = error?.response?.data?.title ?? "Ошибка";
            setErrorState(text);
        }
    };

    return (
        <div>
            <button onClick={() => setEditModel({})}>Добавить</button>
            {successMessage && (
                <Notification
                    message={successMessage}
                    onClose={() => setSuccessMessage(null)}
                />
            )}
            {editModel && <Modal onClose={() => setEditModel(undefined)}>
                <div>{editModel?.id ? 'Редактирование' : "Добавление"} типа кузова</div>
                <Input
                    label='Наименование'
                    value={editModel?.name}
                    onChange={(v) => setEditModel({ ...editModel, name: v as string })}
                />
                <button onClick={onSave}>Сохранить</button>
            </Modal>}
            {errorState && <Modal onClose={() => setErrorState(undefined)}>
                {errorState}
            </Modal>}
            <Table
                currentPage={pageNumber}
                onChangePage={setPageNumber}
                totalCount={entities?.totalCount ?? 0}
                data={entities?.entities ?? []}
                columns={[
                    {
                        header: '',
                        key: 'Id',
                        customCell: (index: number) => <div style={{ cursor: 'pointer' }} onClick={() => onEdit(index)}>Редактировать</div>
                    },
                    {
                        header: "Наименование",
                        key: "name"
                    },
                    {
                        header: "Дата создания",
                        key: "createdOn"
                    },
                    {
                        header: '',
                        key: 'Id',
                        customCell: (index: number) => <div style={{ cursor: 'pointer' }} onClick={() => onDelete(index)}>Удалить</div>
                    },
                ]}
            />
        </div>
    );
};

export default BodyTypePage;